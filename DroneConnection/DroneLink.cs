using System;
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

        public MavLinkConnection mavlink { get; }

        public SerialPort port { get { return mavlink.port; } }

        public volatile ConnectionState state = ConnectionState.UNINITIALIZED;



        public static DroneLink connect(SerialPort port)
        {
            
            MavLinkConnection oConnection = new MavLinkConnection(port);
            Boolean connected = oConnection.connect();
            if (connected)
            {
                // return new connection
                return new DroneLink(oConnection);
            }
            else
            {
                return null;
            }
        }

        private DroneLink(MavLinkConnection mavlink)
        {
            this.mavlink = mavlink;
        }


        public enum ConnectionState
        {
            CONNECTED,
            DISCONNECTED,
            DISCOVERING,
            UNINITIALIZED,
            FAILED
        }
        
        public void close()
        {
            state = ConnectionState.DISCONNECTED;
            mavlink.disconnect();
        }

    }
}
