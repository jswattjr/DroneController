using DroneManager.Models.MessageContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class ServoOutputRawDTO
    {
        /// <summary> Timestamp (microseconds since system boot) </summary>
        public UInt32 time_usec { get; set; }
        /// <summary> Servo output 1 value, in microseconds </summary>
        public UInt16 servo1_raw { get; set; }
        /// <summary> Servo output 2 value, in microseconds </summary>
        public UInt16 servo2_raw { get; set; }
        /// <summary> Servo output 3 value, in microseconds </summary>
        public UInt16 servo3_raw { get; set; }
        /// <summary> Servo output 4 value, in microseconds </summary>
        public UInt16 servo4_raw { get; set; }
        /// <summary> Servo output 5 value, in microseconds </summary>
        public UInt16 servo5_raw { get; set; }
        /// <summary> Servo output 6 value, in microseconds </summary>
        public UInt16 servo6_raw { get; set; }
        /// <summary> Servo output 7 value, in microseconds </summary>
        public UInt16 servo7_raw { get; set; }
        /// <summary> Servo output 8 value, in microseconds </summary>
        public UInt16 servo8_raw { get; set; }
        /// <summary> Servo output port (set of 8 outputs = 1 port). Most MAVs will just use one, but this allows to encode more than 8 servos. </summary>
        public int port { get; set; }

        public ServoOutputRawDTO(ServoOutputRaw source)
        {
            Utilities.CopySimilar.CopyAll(source, this);
        }
    }
}