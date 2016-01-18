using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Collections;
using DroneConnection;
using System.Threading;
using DataAccessLibrary;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Models;
using DroneManager.Models;
using NLog;

namespace DroneManager
{
    public class DroneManagementStation
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        IEntityRepository<DroneEntity> droneRepo = RepositoryFactory.getDroneRepository();
        public List<Drone> connections = new List<Drone>();

        public Drone getById(Guid guid)
        {
            logger.Debug("Fetching Drone Information by Id: {0}", guid.ToString());
            Drone result = connections.FirstOrDefault(drone => drone.id == guid);
            if (null == result)
            {
                logger.Debug("Drone info on {0} not found, checking database.", guid.ToString());
                DroneEntity entity = droneRepo.getById(guid);
                if (null == entity)
                {
                    logger.Debug("Drone info on {0} not found in database.", guid.ToString());
                    return null;
                }
                return new Drone(entity);
            }
            else
            {
                return result;
            }
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
                    DroneLink connection = DroneLink.connect(port);
                    if (null != connection)
                    {
                        // TODO: Look up existing record
                        logger.Debug("Connection established on port {0}", connection.port);
                        Boolean droneExists = false;
                        if (droneExists)
                        {

                        }
                        else
                        {
                            logger.Debug("Mavlink device on port {0} is new, creating database entry.", connection.port);
                            Drone drone = new Drone();
                            drone.serialPort = connection.port.PortName;
                            drone.name = connection.mavlink.systemId.ToString();
                            drone.connection = connection;
                            drone.copy(droneRepo.create((DroneEntity)drone));
                            this.connections.Add(drone);
                        }
                    }
                }
            }
        }

    }
}
