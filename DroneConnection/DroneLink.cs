﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using NLog;

namespace DroneConnection
{
    public class DroneLink
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static DroneLink connect(SerialPort port)
        {
            logger.Debug("Attempting connection on port {0}", port.PortName);
            DroneLink oConnection = new DroneLink(port);
            oConnection.heartbeatThread = new Thread(new ThreadStart(oConnection.doWork));
            oConnection.heartbeatThread.Start();
            return oConnection;
        }

        private DroneLink(SerialPort port)
        {
            this.port = port;
        }

        MAVLink.MavlinkParse mavlink = new MAVLink.MavlinkParse();

        public enum ConnectionState
        {
            CONNECTED,
            DISCONNECTED,
            DISCOVERING,
            UNINITIALIZED,
            FAILED
        }

        // 9600, 14400, 19200, 28800, 38400, 57600, 115200
        public int baudValue = 57600;

        public SerialPort port { get; }
        public Guid id { get; }

        public volatile ConnectionState state = ConnectionState.UNINITIALIZED;

        Thread heartbeatThread;

        public void close()
        {
            logger.Debug("Closing connection on port {0}.", port.PortName);
            state = ConnectionState.DISCONNECTED;
            heartbeatThread.Join();
            port.Close();
        }

        void doWork()
        {
            logger.Debug("Starting Discovery on port {0}", port.PortName);
            this.state = ConnectionState.DISCOVERING;
            try
            {
                if (port.IsOpen)
                {
                    logger.Debug("Port {0} is open, closing.", port.PortName);
                    port.Close();
                }

                // set the comport options
                //port.PortName = ;
                logger.Debug("Setting baud value on port {0} to {1}", port.PortName, baudValue);
                port.BaudRate = baudValue;

                // open the comport
                logger.Debug("Attempting to open port {0}", port.PortName);
                port.Open();
            }
            catch (Exception e)
            {
                logger.Error("Exception while attempting to connect to port {0}. Message: {1}", port.PortName, e.Message);
                this.state = ConnectionState.FAILED;
            }

            try
            {
                // set timeout to 2 seconds
                port.ReadTimeout = 2000;

                // does this code do anything???!?
                // request streams - asume target is at 1,1
                /*mavlink.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.REQUEST_DATA_STREAM,
                    new MAVLink.mavlink_request_data_stream_t()
                    {
                        req_message_rate = 2,
                        req_stream_id = (byte)MAVLink.MAV_DATA_STREAM.ALL,
                        start_stop = 1,
                        target_component = 1,
                        target_system = 1
                    });
                */
                while (port.IsOpen)
                {
                    logger.Debug("Port {0} is open. Setting state to CONNECTED.", port.PortName);
                    state = ConnectionState.CONNECTED;

                    logger.Debug("Sending heartbeat packet to port {0}", port.PortName);
                    // try read a hb packet from the comport
                    var hb = readsomedata<MAVLink.mavlink_heartbeat_t>();

                    var att = readsomedata<MAVLink.mavlink_attitude_t>();

                    Thread.Sleep(1);

                    // this state can be changed externally to signal a disconnect
                    if (state.Equals(ConnectionState.DISCONNECTED))
                    {
                        break;
                    }
                }

                logger.Debug("Connection to Port {0} ending.", port.PortName);
                logger.Debug("Connection state: {0}.", state);
            }
            catch (Exception e)
            {
                logger.Debug("Connection FAILED for port {0} with message: {1}", port.PortName, e.Message);
                state = ConnectionState.FAILED;
            }
            finally
            {
                logger.Debug("Closing port {0}", port.PortName);
                port.Close();
            }
        }

        T readsomedata<T>(int timeout = 2000)
        {
            DateTime deadline = DateTime.Now.AddMilliseconds(timeout);

            // read the current buffered bytes
            while (DateTime.Now < deadline)
            {
                var packet = mavlink.ReadPacketObj(port.BaseStream);

                if (packet == null)
                    continue;

                Console.WriteLine(packet);

                if (packet.GetType() == typeof(T))
                {
                    return (T)packet;
                }
            }

            throw new Exception("No packet match found");
        }

    }
}
