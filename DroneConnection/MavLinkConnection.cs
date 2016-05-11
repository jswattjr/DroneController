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
        static Logger logger = LogManager.GetLogger("applog");

        // serial port where communication is taking place
        private SerialPort port { get; set; }

        // MAVLink library parse helper
        MAVLink.MavlinkParse mavlinkParse = new MAVLink.MavlinkParse();

        // thread listening to port
        Thread listenThread;

        // Events object posting messages to RabbitMQ events broker
        MavLinkEvents events { get; set; }

        // 9600, 14400, 19200, 28800, 38400, 57600, 115200
        public int baudValue = 57600;

        // time to sleep between stream read attempts
        private const int sleeptime_ms = 1;
        // time to read for new packets
        private const int readtime_ms = 1000;
        // timeout for port communication
        private const int timeout_ms = 2000;

        // this is the id of the connected system (fetched from MavLinkMessage sysid field)
        public int systemId = -1;
        public int componentId = -1;

        // static constructor/initializer, attempts a connection and returns null if connection fails
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

        public MavLinkConnection(SerialPort port)
        {
            this.port = port;
        }

        ~MavLinkConnection()
        {
            this.disconnect();
        }

        // attempts port connect and spawns listening thread
        public Boolean connect ()
        {
            logger.Debug("Attempting connection on port {0}", port.PortName);
            try
            {
                if (port.IsOpen)
                {
                    logger.Debug("Port {0} is already open, disconnecting...", port.PortName);
                    this.disconnect();
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
                this.disconnect();
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
                if (count >= timeout_ms)
                {
                    logger.Error("Timed out waiting for response after {0} ms on port {1}", timeout_ms, port.PortName);
                    listenThread.Abort();
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean isOpen()
        {
            if (null == port)
            {
                return false;
            }
            return this.port.IsOpen;
        }

        public String portName()
        {
            if (null == port)
            {
                return "";
            }
            return this.port.PortName;
        }

        // listening thread, reads from serial port and populates readQueue
        private void listen()
        {
            try
            {
                logger.Debug("Starting listening thread for port {0}.", port.PortName);
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
                logger.Debug("StackTrace: {0}", e.StackTrace.ToString());
            }
            finally
            {
                if (null != events)
                {
                    events.destroyQueue();
                }
                this.disconnect();
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
                if (message.messid.Equals(MAVLink.MAVLINK_MSG_ID.HEARTBEAT))
                {
                    logger.Trace("{1} Message read from target system: {0}, component id {3}, of type {2}", message.sysid, message.messid, message.getMessageType().ToString(), message.compid);
                }
                logger.Trace("{1} Message read from target system: {0}, component id {3}, of type {2}", message.sysid, message.messid, message.getMessageType().ToString(), message.compid);
                this.systemId = message.sysid;
                this.componentId = message.compid;
                this.postMessageToLocalEventQueue(message);

                // respond to heartbeat messages with heartbeat of our own
                if (message.messid.Equals(MAVLink.MAVLINK_MSG_ID.HEARTBEAT))
                {
                    sendHeartbeat();
                }
            }
        }

        private void postMessageToLocalEventQueue(MavLinkMessage message)
        {
            // don't post any messages if we don't know who we're communicating with
            if (-1 == this.systemId )
            {
                return;
            }

            // create events object
            if (null == events)
            {
                this.events = new MavLinkEvents(this.systemId, this.componentId);
            }

            events.postToMessageQueue(message);
        }

        // parses data from stream into MavLinkMessage objects
        MavLinkMessage ReadMessage(Stream BaseStream)
        {
            byte[] buffer = this.mavlinkParse.ReadPacket(BaseStream);
            if (buffer.Length >= 6)
            {
                MavLinkMessage message = new MavLinkMessage(buffer);
                return message;
            }
            logger.Error("Bad Message recieved with size less than 6: ", System.Text.Encoding.Default.GetString(buffer));
            return null;
        }

        public void sendParamUpdate(string parameterName, float parameterValue, MAVLink.MAV_PARAM_TYPE type)
        {
            MAVLink.mavlink_param_set_t paramset = new MAVLink.mavlink_param_set_t();
            paramset.target_component = (byte)this.componentId;
            paramset.target_system = (byte)this.systemId;
            paramset.param_value = parameterValue;

            // have to set fixed 16 byte value for param_id
            byte[] byteArray = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                if (i < parameterName.Length)
                {
                    byteArray[i] = (byte)parameterName[i];
                }
                else
                {
                    byteArray[i] = (byte)'\0';
                }
            }
            paramset.param_id = byteArray;

            paramset.param_type = (byte)type;

            byte[] packet = this.mavlinkParse.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.PARAM_SET, paramset);

            this.port.Write(packet, 0, packet.Length);
        }

        private void sendHeartbeat()
        {
            MAVLink.mavlink_heartbeat_t heartbeat = new MAVLink.mavlink_heartbeat_t();
            heartbeat.autopilot = (byte)MAVLink.MAV_AUTOPILOT.INVALID;
            heartbeat.type = (byte)MAVLink.MAV_TYPE.GCS;

            byte[] packet = this.mavlinkParse.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.HEARTBEAT, heartbeat);

            this.port.Write(packet, 0, packet.Length);
        }

        public void sendCommand(ushort commandType, params Int32[] parameters)
        {
            MAVLink.mavlink_command_long_t req = new MAVLink.mavlink_command_long_t();

            req.target_system = Convert.ToByte(this.systemId);
            req.target_component = Convert.ToByte(this.componentId);

            req.command = commandType;

            // this loop takes input parameters and assigns them to the correct part of the struct
            // not sure how to do this better without reflection, since none of these are nullable
            // and I don't want to set values unnecessarily
            for (int paramIndex = 0; paramIndex < parameters.Length; paramIndex++)
            {
                Int32 parameter = parameters[paramIndex];
                if (paramIndex == 1)
                {
                    req.param1 = parameter;
                }
                if (paramIndex == 2)
                {
                    req.param2 = parameter;
                }
                if (paramIndex == 3)
                {
                    req.param3 = parameter;
                }
                if (paramIndex == 4)
                {
                    req.param4 = parameter;
                }
                if (paramIndex == 5)
                {
                    req.param5 = parameter;
                }
                if (paramIndex == 6)
                {
                    req.param6 = parameter;
                }
                if (paramIndex == 7)
                {
                    req.param7 = parameter;
                }
            }


            byte[] packet = this.mavlinkParse.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);

            this.port.Write(packet, 0, packet.Length);
  
        }

        public void sendParamsListRequest()
        {
            MAVLink.mavlink_param_request_list_t paramsRequest = new MAVLink.mavlink_param_request_list_t();
            paramsRequest.target_system = (byte)this.systemId;
            paramsRequest.target_component = (byte)this.componentId;

            byte[] packet = this.mavlinkParse.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.PARAM_REQUEST_LIST, paramsRequest);

            this.port.Write(packet, 0, packet.Length);
        }


    }
}
