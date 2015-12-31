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

namespace DroneManager
{
    public class DroneManagementStation
    {
        IEntityRepository<DroneEntity> droneRepo = RepositoryFactory.getDroneRepository();
        public List<Drone> connections = new List<Drone>();
        public List<Task<DroneLink>> discoveryTasks = new List<Task<DroneLink>>();

        public Drone getById(Guid guid)
        {
            Drone result = connections.FirstOrDefault(drone => drone.id == guid);
            if (null == result)
            {
                DroneEntity entity = droneRepo.getById(guid);
                if (null == entity)
                {
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
            checkTasks();
            string[] ports = SerialPort.GetPortNames();
            foreach (string portName in ports)
            {
                SerialPort port = new SerialPort(portName);
                if (!port.IsOpen)
                {
                    discoveryTasks.Add(Task<DroneLink>.Factory.StartNew(() => DroneLink.connect(port)));
                }
            }
        }

        public void checkTasks()
        {
            List<Task<DroneLink>> processedTasks = new List<Task<DroneLink>>();
            foreach (Task<DroneLink> task in discoveryTasks)
            {
                DroneLink connection = task.Result;
                if (connection.state.Equals(DroneLink.ConnectionState.CONNECTED))
                {
                    // TODO: Look up existing record
                    Boolean droneExists = false;
                    if (droneExists)
                    {

                    }
                    else
                    {
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
