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
            Drone result = connections.FirstOrDefault(drone => drone.data.id == guid);
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
                    MavLinkConnection connection = MavLinkConnection.createConnection(port);
                    if ((null != connection)&&(connection.port.IsOpen))
                    {
                        // TODO: Look up existing record
                        logger.Debug("Connection established on port {0}", connection.port.PortName);
                        DroneEntity connectionRecord = droneRepo.getByName(connection.systemId.ToString());
                        if (null != connectionRecord)
                        {
                            logger.Debug("Mavlink device with systemid {0} found in record with guid {1}", connection.systemId, connectionRecord.id);
                        }
                        else
                        {
                            logger.Debug("Mavlink device on port {0} is new, creating database entry.", connection.port);

                            // create record of this connection
                            connectionRecord = new DroneEntity();
                            connectionRecord.serialPort = connection.port.PortName;
                            connectionRecord.name = connection.systemId.ToString();
                            connectionRecord.copy(droneRepo.create(connectionRecord));
                        }
                        // create new business object around this record and assign it to connection
                        Drone drone = new Drone(connectionRecord);
                        drone.connection = connection;

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
