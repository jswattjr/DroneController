using DroneManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneController.DataTransferObjects
{
    public class DroneDTO
    {
        public Guid id { get; }
        public String name { get; }
        public String port { get; }

        public ConnectionState state { get; }

        public enum ConnectionState
        {
            CONNECTED,
            DISCONNECTED
        }

        public DroneDTO(Drone droneObj)
        {
            this.id = droneObj.data.id;
            this.name = droneObj.data.name;
            this.port = droneObj.data.serialPort;
            if (null != droneObj.connection)
            {
                if (droneObj.connection.port.IsOpen)
                {
                    this.state = ConnectionState.CONNECTED;
                }
                else
                {
                    this.state = ConnectionState.DISCONNECTED;
                }
            }
        }
    }
}