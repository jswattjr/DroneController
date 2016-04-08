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
        public String command { get; set; }
        /// <summary> See MAV_RESULT enum </summary>
        public String result { get; set; }
    }
}