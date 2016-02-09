using DroneManager.Models;
using DroneManager.Models.MessageContainers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        public HeartbeatDTO heartbeat_data { get; }

        public SystemStatusDTO sys_status_data { get; }

        public SystemTimeDTO sys_time_data { get; }

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
            this.heartbeat_data = new HeartbeatDTO(droneObj.getHearbeat());
            this.sys_status_data = new SystemStatusDTO(droneObj.getSystemStatus());
            this.sys_time_data = new SystemTimeDTO(droneObj.getSystemTime());
        }

    }
}