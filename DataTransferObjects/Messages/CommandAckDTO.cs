using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class CommandAckDTO
    {
        /// <summary> Command ID, as defined by MAV_CMD enum. </summary>
        public MAVLink.MAV_CMD command { get; set; }
        /// <summary> See MAV_RESULT enum </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public MAVLink.MAV_RESULT result { get; set; }
    }
}