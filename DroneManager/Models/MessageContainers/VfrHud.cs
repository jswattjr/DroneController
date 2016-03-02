using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
    VFR_HUD ( #74 )

    Metrics typically displayed on a HUD for fixed wing aircraft

    Field Name	Type	Description
    airspeed	float	Current airspeed in m/s
    groundspeed	float	Current ground speed in m/s
    heading	int16_t	Current heading in degrees, in compass units (0..360, 0=north)
    throttle	uint16_t	Current throttle setting in integer percent, 0 to 100
    alt	float	Current altitude (MSL), in meters
    climb	float	Current climb rate in meters/second
    */

    public class VfrHud : MessageContainerBase
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

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#VFR_HUD";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.VFR_HUD;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_vfr_hud_t);
        }

        public VfrHud(MavLinkMessage message) : base(null)
        {
            if (message.messid.Equals(this.MessageID))
            {
                MAVLink.mavlink_vfr_hud_t data = (MAVLink.mavlink_vfr_hud_t)message.data_struct;
                this.airspeed = data.airspeed;
                this.alt = data.alt;
                this.climb = data.climb;
                this.groundspeed = data.groundspeed;
                this.heading = data.heading;
                this.throttle = data.throttle;
            }
        }
    }
}
