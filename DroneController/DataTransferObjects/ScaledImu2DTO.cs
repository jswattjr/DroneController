using DroneManager.Models.MessageContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneController.DataTransferObjects
{
    public class ScaledImu2DTO
    {
        /// <summary> Timestamp (milliseconds since system boot) </summary>
        public UInt32 time_boot_ms { get; set; }
        /// <summary> X acceleration (mg) </summary>
        public Int16 xacc { get; set; }
        /// <summary> Y acceleration (mg) </summary>
        public Int16 yacc { get; set; }
        /// <summary> Z acceleration (mg) </summary>
        public Int16 zacc { get; set; }
        /// <summary> Angular speed around X axis (millirad /sec) </summary>
        public Int16 xgyro { get; set; }
        /// <summary> Angular speed around Y axis (millirad /sec) </summary>
        public Int16 ygyro { get; set; }
        /// <summary> Angular speed around Z axis (millirad /sec) </summary>
        public Int16 zgyro { get; set; }
        /// <summary> X Magnetic field (milli tesla) </summary>
        public Int16 xmag { get; set; }
        /// <summary> Y Magnetic field (milli tesla) </summary>
        public Int16 ymag { get; set; }
        /// <summary> Z Magnetic field (milli tesla) </summary>
        public Int16 zmag { get; set; }

        public ScaledImu2DTO (ScaledImu2 source)
        {
            Utilities.CopySimilar.CopyAll(source, this);
        }
    }
}