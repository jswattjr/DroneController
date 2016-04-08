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
        public String type { get; set; }
        public String autopilot { get; set; }
        public UInt32 custom_mode { get; set; }
        public List<String> base_mode { get; set; }
        public String system_status { get; set; }
        public int mavlink_version { get; set; }
    }
}