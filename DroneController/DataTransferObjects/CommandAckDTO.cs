using DroneManager.Models.MessageContainers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneController.DataTransferObjects
{
    public class CommandAckDTO
    {
        /// <summary> Command ID, as defined by MAV_CMD enum. </summary>
        public MAVLink.MAV_CMD command { get; set; }
        /// <summary> See MAV_RESULT enum </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public MAVLink.MAV_RESULT result { get; set; }

        public CommandAckDTO(CommandAck source)
        {
            Utilities.CopySimilar.CopyAll(source, this);
        }
    }
}