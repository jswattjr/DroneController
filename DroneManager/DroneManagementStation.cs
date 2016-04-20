using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Collections;
using DroneConnection;
using System.Threading;
using DroneManager.Models;
using NLog;
using Utilities;

namespace DroneManager
{
    public class DroneManagementStation
    {
        Logger logger = LogManager.GetLogger("applog");

        SafeList<Drone> connections = new SafeList<Drone>();
        public List<Drone> Connections
        {
            get
            {
                lock (connections)
                {
                    return new List<Drone>(connections);
                }
            }
        }


        public Drone getById(Guid guid)
        {
            logger.Debug("Fetching Drone Information by Id: {0}", guid.ToString());
            Drone result = Connections.FirstOrDefault(drone => drone.id == guid);
            if (null == result)
            {
                logger.Debug("Drone info on {0} not found", guid.ToString());
                return null;
            }
            else
            {
                return result;
            }
        }

        public Boolean disconnect(Guid guid)
        {
            logger.Debug("Disconnecting Drone: {0}", guid.ToString());
            Drone result = Connections.FirstOrDefault(drone => drone.id == guid);
            if (null != result)
            {
                this.connections.Remove(result);
                result.connection.disconnect();
                return true;
            }
            return false;
        }

        public void discover()
        {
            logger.Debug("Starting discovery.");
            string[] ports = SerialPort.GetPortNames();
            foreach (string portName in ports)
            {
                logger.Debug("Checking port {0}", portName);
                SerialPort port = new SerialPort(portName);
                if (!port.IsOpen)
                {
                    logger.Debug("Port {0} is currently closed, attempting new connection.", portName);
                    MavLinkConnection connection = MavLinkConnection.createConnection(port);
                    if ((null != connection)&&(connection.isOpen()))
                    {
                        // TODO: Look up existing record
                        logger.Debug("Connection established on port {0}", connection.portName());
                        logger.Debug("Mavlink device on port {0} is new, creating database entry.", connection.portName());

                        // create new business object around this record and assign it to connection
                        Drone drone = new Drone(connection);

                        // add this object to the list of active connections
                        this.connections.Add(drone);

                    }
                    else
                    {
                        logger.Debug("Discontinuing connection attempt on port {0}", portName);
                    }
                }
            }
        }

    }
}
