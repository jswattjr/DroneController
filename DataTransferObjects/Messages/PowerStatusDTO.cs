using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class PowerStatusDTO
    {
        /// <summary> 5V rail voltage in millivolts </summary>
        public UInt16 Vcc { get; set; }
        /// <summary> servo rail voltage in millivolts </summary>
        public UInt16 Vservo { get; set; }
        /// <summary> power supply status flags (see MAV_POWER_STATUS enum) </summary>
        //public UInt16 flags;
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<MAVLink.MAV_POWER_STATUS> flags { get; set; }
    }
}