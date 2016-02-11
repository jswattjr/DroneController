using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
    ATTITUDE ( #30 )

        The attitude in the aeronautical frame (right-handed, Z-down, X-front, Y-right).

        Field Name	Type	Description
        time_boot_ms	uint32_t	Timestamp (milliseconds since system boot)
        roll	float	Roll angle (rad, -pi..+pi)
        pitch	float	Pitch angle (rad, -pi..+pi)
        yaw	float	Yaw angle (rad, -pi..+pi)
        rollspeed	float	Roll angular speed (rad/s)
        pitchspeed	float	Pitch angular speed (rad/s)
        yawspeed	float	Yaw angular speed (rad/s)
    */
    public class Attitude : MessageContainerBase
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

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#ATTITUDE";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.ATTITUDE;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_attitude_t);
        }

        public Attitude(MavLinkMessage message) : base(null)
        {
            if (message.messid == this.MessageID)
            {
                MAVLink.mavlink_attitude_t data = (MAVLink.mavlink_attitude_t)message.data_struct;
                this.pitch = data.pitch;
                this.pitchspeed = data.pitchspeed;
                this.roll = data.roll;
                this.rollspeed = data.rollspeed;
                this.time_boot_ms = data.time_boot_ms;
                this.yaw = data.yaw;
                this.yawspeed = data.yawspeed;
            }
        }
    }
}
