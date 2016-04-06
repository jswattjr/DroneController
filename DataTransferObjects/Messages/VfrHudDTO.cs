using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class VfrHudDTO
    {
        /// <summary> Current airspeed in m/s </summary>
        public Single airspeed { get; set; }
        /// <summary> Current ground speed in m/s </summary>
        public Single groundspeed { get; set; }
        /// <summary> Current altitude (MSL), in meters </summary>
        public Single alt { get; set; }
        /// <summary> Current climb rate in meters/second </summary>
        public Single climb { get; set; }
        /// <summary> Current heading in degrees, in compass units (0..360, 0=north) </summary>
        public Int16 heading { get; set; }
        /// <summary> Current throttle setting in integer percent, 0 to 100 </summary>
        public UInt16 throttle { get; set; }
    }
}