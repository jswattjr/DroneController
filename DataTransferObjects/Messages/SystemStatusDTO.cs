using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class SystemStatusDTO
    {
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<MAVLink.MAV_SYS_STATUS_SENSOR> sensorsPresent = new List<MAVLink.MAV_SYS_STATUS_SENSOR>();
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<MAVLink.MAV_SYS_STATUS_SENSOR> sensorsEnabled = new List<MAVLink.MAV_SYS_STATUS_SENSOR>();
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<MAVLink.MAV_SYS_STATUS_SENSOR> sensorsHealth = new List<MAVLink.MAV_SYS_STATUS_SENSOR>();
        public UInt16 voltage_battery {get; set;}
        public Int16 current_battery { get; set; }
        public Int16 battery_remaining { get; set; }
        public UInt16 drop_rate_comm { get; set; }
        public UInt16 errors_comm { get; set; }
        public UInt16 errors_count1 { get; set; }
        public UInt16 errors_count2 { get; set; }
        public UInt16 errors_count3 { get; set; }
        public UInt16 errors_count4 { get; set; }
    }
}