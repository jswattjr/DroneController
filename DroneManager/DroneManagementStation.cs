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
        public List<Task<DroneLink>> discoveryTasks = new List<Task<DroneLink>>();

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
            checkTasks();
            string[] ports = SerialPort.GetPortNames();
            foreach (string portName in ports)
            {
                logger.Debug("Checking port {0}", portName);
                SerialPort port = new SerialPort(portName);
                if (!port.IsOpen)
                {
                    logger.Debug("Port {0} is currently closed, attempting connection.", portName);
                    discoveryTasks.Add(Task<DroneLink>.Factory.StartNew(() => DroneLink.connect(port)));
                }
            }
        }

        public void checkTasks()
        {
            logger.Debug("Checking existing connections.");
            List<Task<DroneLink>> processedTasks = new List<Task<DroneLink>>();
            foreach (Task<DroneLink> task in discoveryTasks)
            {
                DroneLink connection = task.Result;
                if (connection.state.Equals(DroneLink.ConnectionState.CONNECTED))
                {
                    // TODO: Look up existing record
                    logger.Debug("Connection established on port {0}", connection.port);
                    Boolean droneExists = false;
                    if (droneExists)
                    {

                    }
                    else
                    {
                        logger.Debug("Connection on port {0} is new, creating database entry.", connection.port);
                        Drone drone = new Drone();
                        drone.serialPort = connection.port.PortName;
                        drone.connection = connection;
                        drone.copy(droneRepo.create((DroneEntity)drone));
                        this.connections.Add(drone);
                    }
                    processedTasks.Add(task);
                }
                else if (connection.state.Equals(DroneLink.ConnectionState.DISCOVERING))
                {
                    continue;
                }
                else
                {
                    // TODO: log removal of task
                    logger.Debug("Connection on port {0} is {1}.", connection.port, connection.state.ToString());
                    processedTasks.Add(task);
                }
            }
            foreach (Task<DroneLink> processedTask in processedTasks)
            {
                discoveryTasks.Remove(processedTask);
            }
        }
    }
}
