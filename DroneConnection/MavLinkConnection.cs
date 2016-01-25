using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Collections.Concurrent;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace DroneConnection
{
    /*
        This class is responsible for connecting via Mavlink library over the serial port (usb) provided.
        On a successful connection, it will spawn a thread that will capture (read) messages from the mavlink connection
        and persist them into a fixed queue of 1000 messages.
    */
    public class MavLinkConnection
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static MavLinkConnection createConnection(SerialPort port)
        {
            MavLinkConnection oConnection = new MavLinkConnection(port);
            Boolean connected = oConnection.connect();
            if (connected)
            {
                // return new connection
                return oConnection;
            }
            else
            {
                return null;
            }
        }

        public SerialPort port { get; }
        MAVLink.MavlinkParse mavlinkParse = new MAVLink.MavlinkParse();
        Thread listenThread;

        // 9600, 14400, 19200, 28800, 38400, 57600, 115200
        public int baudValue = 57600;

        // time to sleep between stream read attempts
        private const int sleeptime_ms = 1;
        // time to read for new packets
        private const int readtime_ms = 1000;
        // timeout for port communication
        private const int timeout_ms = 2000;
        // size of message queue to store
        private const int messageQueueSize = 1000;

        // each message will be stored here, and messages older than messageQueueSize will be removed
        public FixedSizedQueue<MavLinkMessage> readQueue = new FixedSizedQueue<MavLinkMessage>(messageQueueSize);

        // this is the id of the connected system (fetched from MavLinkMessage sysid field)
        public int systemId = -1;
        int componentId = -1;

        // RabbitMQ capabilities, CreateMessageQueue to initialize, DisposeMessageQueue to delete
        ConnectionFactory eventFactory = new ConnectionFactory();
        IConnection connection = null;
        IModel channel = null;
        QueueDeclareOk messageQueueExists = null;
        const String messageQueuePrefix = "MavLinkConnection";

        public MavLinkConnection(SerialPort port)
        {
            this.port = port;
        }

        // attempts port connect and spawns listening thread
        public Boolean connect ()
        {
            logger.Debug("Attempting connection on port {0}", port.PortName);
            try
            {
                if (port.IsOpen)
                {
                    logger.Debug("Port {0} is open, closing.", port.PortName);
                    port.Close();
                }

                // set the comport options
                logger.Debug("Setting baud value on port {0} to {1}", port.PortName, baudValue);
                port.BaudRate = baudValue;

                // open the comport
                logger.Debug("Attempting to open port {0}", port.PortName);
                port.Open();
            }
            catch (Exception e)
            {
                logger.Error("Exception while attempting to connect to port {0}. Message: {1}", port.PortName, e.Message);
                port.Close();
                return false;
            }

            if (port.IsOpen)
            {
                // set timeout to 2 seconds
                port.ReadTimeout = MavLinkConnection.timeout_ms;

                // start thread to listen for incoming messages, refers to private listen() function
                listenThread = new Thread(new ThreadStart(listen));
                listenThread.Start();

                // wait for connection to fetch system id of target
                int count = 0;
                while ((this.systemId == -1) && (count < timeout_ms))
                {
                    Thread.Sleep(1);
                    count++;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        // listening thread, reads from serial port and populates readQueue
        private void listen()
        {
            try
            {
                logger.Debug("Starting listening thread for port {0}.", port.PortName);

                this.initializeMessageQueue();

                while (port.IsOpen)
                {

                    readFromStream();

                    Thread.Sleep(MavLinkConnection.sleeptime_ms);
                }

                logger.Debug("Connection to Port {0} ending.", port.PortName);
            }
            catch (Exception e)
            {
                logger.Debug("Connection FAILED for port {0} with message: {1}", port.PortName, e.Message);
            }
            finally
            {
                logger.Debug("Closing port {0}", port.PortName);
                this.destroyQueue();
                port.Close();
            }

        }

        // closes the serial port, this should have the side effect of killing the listening thread (which should be in a while(port.IsOpen) loop)
        public void disconnect()
        {
            logger.Debug("Closing connection on port {0}.", port.PortName);
            port.Close();
        }

        // reads for readtime_ms and calls ReadMessage() on each packet from connection, stores messages in the object member 'readQueue'
        void readFromStream()
        {
            DateTime deadline = DateTime.Now.AddMilliseconds(MavLinkConnection.readtime_ms);

            // read the current buffered bytes
            while (DateTime.Now < deadline)
            {
                MavLinkMessage message = ReadMessage(port.BaseStream);

                if (message == null)
                {
                    continue;
                }

                logger.Debug("{1} Message read from target system: {0}, component id {3}, of type {2}", message.sysid, message.messid, message.getMessageType().ToString(), message.compid);
                this.systemId = message.sysid;
                this.componentId = message.compid;
                this.readQueue.Enqueue(message);
                this.postToMessageQueue(message);
            }
        }

        // parses data from stream into MavLinkMessage objects
        MavLinkMessage ReadMessage(Stream BaseStream)
        {
            byte[] buffer = this.mavlinkParse.ReadPacket(BaseStream);
            MavLinkMessage message = new MavLinkMessage(buffer);
            return message;
        }

        public static String getMessageQueueName(int systemId)
        {
            return messageQueuePrefix + "_" + systemId.ToString();
        }

        public String getMessageQueueName()
        {
            return MavLinkConnection.getMessageQueueName(this.systemId);
        }

        // initializes queue variables, needs to be paired with 'destroyqueue' to release resources
        void initializeMessageQueue()
        {
            try
            {
                IConnection connection = eventFactory.CreateConnection();
                IModel channel = connection.CreateModel();
                logger.Debug("RabbitMQ message queue created successfully.");
            }
            catch (Exception e)
            {
                logger.Debug("Exception while initializing message queue. Please check that target RabbitMQ server exists.");
                logger.Debug("Exception message: {0}", e.Message);
                if (null != connection)
                {
                    connection.Dispose();
                }
                if (null != channel)
                {
                    channel.Dispose();
                }
                connection = null;
                channel = null;
            }
        }

        // declares the message queue, needs to be called after sysid is fetched from target system, so id can be used in queue name
        void declareQueue()
        {
            if ((null == channel)||(null == connection))
            {
                return;
            }
            this.messageQueueExists = channel.QueueDeclare(
                queue: getMessageQueueName(),
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            logger.Debug("RabbitMQ Message Queue Created for {0}", port.PortName);
        }

        // releases queue resources
        void destroyQueue()
        {
            if ((null == channel) || (null == connection))
            {
                return;
            }
            connection.Dispose();
            channel.Dispose();
        } 

        // function for posting message to queue
        void postToMessageQueue(MavLinkMessage message)
        {
            if ((null == channel) || (null == connection))
            {
                return;
            }
            if (null == this.messageQueueExists)
            {
                this.declareQueue();
            }

            // rabbitmq only supports string, so convert to JSON then drop it in
            String jsonObject = JsonConvert.SerializeObject(message);

            // message properties
            IBasicProperties props = channel.CreateBasicProperties();

            // encode message
            var messageBody = Encoding.UTF8.GetBytes(jsonObject);

            // publish message
            this.channel.BasicPublish("", this.getMessageQueueName(), props, messageBody);
            logger.Debug("RabbitMQ Message {1} published for {0}", port.PortName, messageBody);
        }


        public void sendArmMessage(Boolean armValue = true)
        {
            MAVLink.mavlink_command_long_t req = new MAVLink.mavlink_command_long_t();

            req.target_system = Convert.ToByte(this.systemId);
            req.target_component = Convert.ToByte(this.componentId);

            req.command = (ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM;

            req.param1 = Convert.ToInt32(armValue);

            byte[] packet = mavlinkParse.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);

            port.Write(packet, 0, packet.Length);

            /*
            // Implement Async Ack code?
            try
            {
                var ack = readsomedata<MAVLink.mavlink_command_ack_t>();
                if (ack.result == (byte)MAVLink.MAV_RESULT.ACCEPTED)
                {

                }
            }
            catch
            {
            }
            */
        }
    }
}
