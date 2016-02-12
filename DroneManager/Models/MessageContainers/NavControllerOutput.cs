using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
    NAV_CONTROLLER_OUTPUT ( #62 )

    Outputs of the APM navigation controller. The primary use of this message is to check the response and signs of the controller before actual flight and to assist with tuning controller parameters.

    Field Name	Type	Description
    nav_roll	float	Current desired roll in degrees
    nav_pitch	float	Current desired pitch in degrees
    nav_bearing	int16_t	Current desired heading in degrees
    target_bearing	int16_t	Bearing to current MISSION/target in degrees
    wp_dist	uint16_t	Distance to active MISSION in meters
    alt_error	float	Current altitude error in meters
    aspd_error	float	Current airspeed error in meters/second
    xtrack_error	float	Current crosstrack error on x-y plane in meters
    */
    public class NavControllerOutput : MessageContainerBase
    {
        /// <summary> Current desired roll in degrees </summary>
        public Single nav_roll;
        /// <summary> Current desired pitch in degrees </summary>
        public Single nav_pitch;
        /// <summary> Current altitude error in meters </summary>
        public Single alt_error;
        /// <summary> Current airspeed error in meters/second </summary>
        public Single aspd_error;
        /// <summary> Current crosstrack error on x-y plane in meters </summary>
        public Single xtrack_error;
        /// <summary> Current desired heading in degrees </summary>
        public Int16 nav_bearing;
        /// <summary> Bearing to current MISSION/target in degrees </summary>
        public Int16 target_bearing;
        /// <summary> Distance to active MISSION in meters </summary>
        public UInt16 wp_dist;

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#NAV_CONTROLLER_OUTPUT";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.NAV_CONTROLLER_OUTPUT;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_nav_controller_output_t);
        }

        public NavControllerOutput(MavLinkMessage message) : base(null)
        {
            if (message.messid.Equals(this.MessageID))
            {
                MAVLink.mavlink_nav_controller_output_t data = (MAVLink.mavlink_nav_controller_output_t)message.data_struct;
                this.alt_error = data.alt_error;
                this.aspd_error = data.aspd_error;
                this.nav_bearing = data.nav_bearing;
                this.nav_pitch = data.nav_pitch;
                this.nav_roll = data.nav_roll;
                this.xtrack_error = data.xtrack_error;
                this.wp_dist = data.wp_dist;
                this.target_bearing = data.target_bearing;
            }
        }
    }
}
