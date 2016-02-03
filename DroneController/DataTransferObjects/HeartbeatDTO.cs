using DroneManager.Models.MessageContainers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneController.DataTransferObjects
{
    public class HeartbeatDTO
    {
        // heartbeat parameters
        [JsonConverter(typeof(StringEnumConverter))]
        public MAVLink.MAV_TYPE type { get; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MAVLink.MAV_AUTOPILOT autopilot { get; }
        public UInt32 custom_mode { get; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MAVLink.MAV_MODE_FLAG base_mode { get; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MAVLink.MAV_STATE system_status { get; }
        public int mavlink_version { get; }

        public HeartbeatDTO(Heartbeat data)
        {
            if (null != data)
            {
                this.type = data.type;
                this.autopilot = data.autopilot;
                this.custom_mode = data.custom_mode;
                this.base_mode = data.base_mode;
                this.system_status = data.system_status;
                this.mavlink_version = data.mavlink_version;
            }
        }
    }
}