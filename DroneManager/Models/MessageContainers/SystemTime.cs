using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
        The system time is the time of the master clock, 
        typically the computer clock of the main onboard computer.

        Field Name	Type	Description
        time_unix_usec	uint64_t	Timestamp of the master clock in microseconds since UNIX epoch.
        time_boot_ms	uint32_t	Timestamp of the component clock since boot time in milliseconds.
    */
    public class SystemTime : MessageContainerBase
    {
        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#SYSTEM_TIME";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.SYSTEM_TIME;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_system_time_t);
        }

        public UInt64 time_unix_sec { get; set; }
        public UInt32 time_boot_ms { get; set; }

        public SystemTime(MavLinkMessage message) : base (null)
        {
            MAVLink.mavlink_system_time_t data = (MAVLink.mavlink_system_time_t)message.data_struct;
            this.time_boot_ms = data.time_boot_ms;
            this.time_unix_sec = data.time_unix_usec;
        }
    }
}
