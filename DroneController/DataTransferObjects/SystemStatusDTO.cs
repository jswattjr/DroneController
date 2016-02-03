using DroneManager.Models.MessageContainers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneController.DataTransferObjects
{
    public class SystemStatusDTO
    {
        [JsonConverter(typeof(StringEnumConverter))]
        List<MAVLink.MAV_SYS_STATUS_SENSOR> sensorsPresent = new List<MAVLink.MAV_SYS_STATUS_SENSOR>();
        [JsonConverter(typeof(StringEnumConverter))]
        List<MAVLink.MAV_SYS_STATUS_SENSOR> sensorsEnabled = new List<MAVLink.MAV_SYS_STATUS_SENSOR>();
        [JsonConverter(typeof(StringEnumConverter))]
        List<MAVLink.MAV_SYS_STATUS_SENSOR> sensorsHealth = new List<MAVLink.MAV_SYS_STATUS_SENSOR>();
        UInt16 voltage_battery;
        Int16 current_battery;
        Int16 battery_remaining;
        UInt16 drop_rate_comm;
        UInt16 errors_comm;
        UInt16 errors_count1;
        UInt16 errors_count2;
        UInt16 errors_count3;
        UInt16 errors_count4;

        public SystemStatusDTO(SystemStatus source)
        {
            Utilities.CopySimilar.SetProperties(source, this);
        }
    }
}