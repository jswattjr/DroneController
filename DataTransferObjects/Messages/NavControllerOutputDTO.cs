using DroneManager.Models.MessageContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class NavControllerOutputDTO
    {
        /// <summary> Current desired roll in degrees </summary>
        public Single nav_roll { get; set; }
        /// <summary> Current desired pitch in degrees </summary>
        public Single nav_pitch { get; set; }
        /// <summary> Current altitude error in meters </summary>
        public Single alt_error { get; set; }
        /// <summary> Current airspeed error in meters/second </summary>
        public Single aspd_error { get; set; }
        /// <summary> Current crosstrack error on x-y plane in meters </summary>
        public Single xtrack_error { get; set; }
        /// <summary> Current desired heading in degrees </summary>
        public Int16 nav_bearing { get; set; }
        /// <summary> Bearing to current MISSION/target in degrees </summary>
        public Int16 target_bearing { get; set; }
        /// <summary> Distance to active MISSION in meters </summary>
        public UInt16 wp_dist { get; set; }

        public NavControllerOutputDTO(NavControllerOutput source)
        {
            Utilities.CopySimilar.CopyAll(source, this);
        }
    }
}