using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace DroneConnection
{
    public class DroneLink
    {
        public static DroneLink connect(SerialPort port)
        {
            DroneLink oConnection = new DroneLink(port);
            oConnection.heartbeatThread = new Thread(new ThreadStart(oConnection.doWork));
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

        public ConnectionState state = ConnectionState.UNINITIALIZED;

        Thread heartbeatThread;

        public void close()
        {
            heartbeatThread.Abort();
            heartbeatThread.Join();
            port.Close();
            state = ConnectionState.DISCONNECTED;
        }

        void doWork()
        {
            this.state = ConnectionState.DISCOVERING;
            if (port.IsOpen)
            {
                port.Close();
            }

            // set the comport options
            //port.PortName = ;
            port.BaudRate = baudValue;

            // open the comport
            port.Open();

            // set timeout to 2 seconds
            port.ReadTimeout = 2000;

            // request streams - asume target is at 1,1
            mavlink.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.REQUEST_DATA_STREAM,
                new MAVLink.mavlink_request_data_stream_t()
                {
                    req_message_rate = 2,
                    req_stream_id = (byte)MAVLink.MAV_DATA_STREAM.ALL,
                    start_stop = 1,
                    target_component = 1,
                    target_system = 1
                });

            while (port.IsOpen)
            {
                state = ConnectionState.CONNECTED;
                try
                {
                    // try read a hb packet from the comport
                    var hb = readsomedata<MAVLink.mavlink_heartbeat_t>();

                    var att = readsomedata<MAVLink.mavlink_attitude_t>();
                }
                catch
                {
                    state = ConnectionState.FAILED;
                }

                Thread.Sleep(1);
            }
            state = ConnectionState.FAILED;

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
