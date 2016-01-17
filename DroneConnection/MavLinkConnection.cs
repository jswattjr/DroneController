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

namespace DroneConnection
{
    class MavLinkConnection
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

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
        FixedSizedQueue<MavLinkMessage> readQueue = new FixedSizedQueue<MavLinkMessage>(messageQueueSize);

        // this is the id of the connected system (fetched from MavLinkMessage sysid field)
        public int systemId;

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
                while (port.IsOpen)
                {
                    logger.Debug("Port {0} is open. Setting state to CONNECTED.", port.PortName);

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

                logger.Debug("Message read from target system: {0}, with id {1}, of type {2}", message.sysid, message.messid, message.getMessageType().ToString());
                this.systemId = message.sysid;
                this.readQueue.Enqueue(message);
            }
        }

        // parses data from stream into MavLinkMessage objects
        public MavLinkMessage ReadMessage(Stream BaseStream)
        {
            byte[] buffer = this.mavlinkParse.ReadPacket(BaseStream);
            MavLinkMessage message = new MavLinkMessage(buffer);
            return message;
        }

        public class FixedSizedQueue<T> : ConcurrentQueue<T>
        {
            private readonly object syncObject = new object();

            public int Size { get; private set; }

            public FixedSizedQueue(int size)
            {
                Size = size;
            }

            public new void Enqueue(T obj)
            {
                base.Enqueue(obj);
                lock (syncObject)
                {
                    while (base.Count > Size)
                    {
                        T outObj;
                        base.TryDequeue(out outObj);
                    }
                }
            }
        }


    }
}
