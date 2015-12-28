using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Collections;
using DroneConnection;
using System.Threading;

namespace DroneManager
{
    public class DroneManagementStation
    {
        public List<DroneLink> connections = new List<DroneLink>();
        public void discover()
        {
            string[] ports = SerialPort.GetPortNames();
            List<DroneLink> discoveryThreads = new List<DroneLink>();
            foreach (string portName in ports)
            {
                SerialPort port = new SerialPort(portName);
                if (!port.IsOpen)
                {
                    discoveryThreads.Add(DroneLink.connect(port));
                }
            }

            Thread.Sleep(10);

            foreach (DroneLink link in discoveryThreads){
                if (link.state == DroneLink.ConnectionState.CONNECTED)
                {
                    connections.Add(link);
                }
                else
                {
                    try
                    {
                        link.close();
                    }
                    catch
                    {

                    }
                }
            }

        }

        void connect()
        {

        }
    }
}
