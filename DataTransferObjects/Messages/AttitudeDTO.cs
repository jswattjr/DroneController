using DroneManager.Models.MessageContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class AttitudeDTO
    {
        /// <summary> Timestamp (milliseconds since system boot) </summary>
        public UInt32 time_boot_ms { get; set; }
        /// <summary> Roll angle (rad, -pi..+pi) </summary>
        public Single roll { get; set; }
        /// <summary> Pitch angle (rad, -pi..+pi) </summary>
        public Single pitch { get; set; }
        /// <summary> Yaw angle (rad, -pi..+pi) </summary>
        public Single yaw { get; set; }
        /// <summary> Roll angular speed (rad/s) </summary>
        public Single rollspeed { get; set; }
        /// <summary> Pitch angular speed (rad/s) </summary>
        public Single pitchspeed { get; set; }
        /// <summary> Yaw angular speed (rad/s) </summary>
        public Single yawspeed { get; set; }

        public AttitudeDTO (Attitude source )
        {
            Utilities.CopySimilar.CopyAll(source, this);
        }
    }
}