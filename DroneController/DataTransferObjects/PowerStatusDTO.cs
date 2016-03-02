using DroneManager.Models.MessageContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneController.DataTransferObjects
{
    public class PowerStatusDTO
    {
        /// <summary> 5V rail voltage in millivolts </summary>
        public UInt16 Vcc { get; set; }
        /// <summary> servo rail voltage in millivolts </summary>
        public UInt16 Vservo { get; set; }
        /// <summary> power supply status flags (see MAV_POWER_STATUS enum) </summary>
        //public UInt16 flags;
        public List<MAVLink.MAV_POWER_STATUS> flags { get; set; }

        public PowerStatusDTO (PowerStatus source)
        {
            this.Vcc = source.Vcc;
            this.Vservo = source.Vservo;
            this.flags = new List<MAVLink.MAV_POWER_STATUS>(source.flags);
        }
    }
}