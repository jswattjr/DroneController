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
        public List<Task<DroneLink>> discoveryTasks = new List<Task<DroneLink>>();

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
                    this.connections.Add(connection);
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
