using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class HeartbeatDTO
    {
        // heartbeat parameters
        [JsonConverter(typeof(StringEnumConverter))]
        public MAVLink.MAV_TYPE type { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MAVLink.MAV_AUTOPILOT autopilot { get; set; }
        public UInt32 custom_mode { get; set; }
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<MAVLink.MAV_MODE_FLAG> base_mode { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MAVLink.MAV_STATE system_status { get; set; }
        public int mavlink_version { get; set; }
    }
}