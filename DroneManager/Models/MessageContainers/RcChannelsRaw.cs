using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
    RC_CHANNELS_RAW ( #35 )

    The RAW values of the RC channels received. The standard PPM modulation is as follows: 1000 microseconds: 0%, 2000 microseconds: 100%. Individual receivers/transmitters might violate this specification.

    Field Name	Type	Description
    time_boot_ms	uint32_t	Timestamp (milliseconds since system boot)
    port	uint8_t	Servo output port (set of 8 outputs = 1 port). Most MAVs will just use one, but this allows for more than 8 servos.
    chan1_raw	uint16_t	RC channel 1 value, in microseconds. A value of UINT16_MAX implies the channel is unused.
    chan2_raw	uint16_t	RC channel 2 value, in microseconds. A value of UINT16_MAX implies the channel is unused.
    chan3_raw	uint16_t	RC channel 3 value, in microseconds. A value of UINT16_MAX implies the channel is unused.
    chan4_raw	uint16_t	RC channel 4 value, in microseconds. A value of UINT16_MAX implies the channel is unused.
    chan5_raw	uint16_t	RC channel 5 value, in microseconds. A value of UINT16_MAX implies the channel is unused.
    chan6_raw	uint16_t	RC channel 6 value, in microseconds. A value of UINT16_MAX implies the channel is unused.
    chan7_raw	uint16_t	RC channel 7 value, in microseconds. A value of UINT16_MAX implies the channel is unused.
    chan8_raw	uint16_t	RC channel 8 value, in microseconds. A value of UINT16_MAX implies the channel is unused.
    rssi	uint8_t	Receive signal strength indicator, 0: 0%, 100: 100%, 255: invalid/unknown.
*/
    public class RcChannelsRaw : MessageContainerBase
    {
        /// <summary> Timestamp (milliseconds since system boot) </summary>
        public UInt32 time_boot_ms;
        /// <summary> RC channel 1 value, in microseconds. A value of UINT16_MAX implies the channel is unused. </summary>
        public UInt16 chan1_raw;
        /// <summary> RC channel 2 value, in microseconds. A value of UINT16_MAX implies the channel is unused. </summary>
        public UInt16 chan2_raw;
        /// <summary> RC channel 3 value, in microseconds. A value of UINT16_MAX implies the channel is unused. </summary>
        public UInt16 chan3_raw;
        /// <summary> RC channel 4 value, in microseconds. A value of UINT16_MAX implies the channel is unused. </summary>
        public UInt16 chan4_raw;
        /// <summary> RC channel 5 value, in microseconds. A value of UINT16_MAX implies the channel is unused. </summary>
        public UInt16 chan5_raw;
        /// <summary> RC channel 6 value, in microseconds. A value of UINT16_MAX implies the channel is unused. </summary>
        public UInt16 chan6_raw;
        /// <summary> RC channel 7 value, in microseconds. A value of UINT16_MAX implies the channel is unused. </summary>
        public UInt16 chan7_raw;
        /// <summary> RC channel 8 value, in microseconds. A value of UINT16_MAX implies the channel is unused. </summary>
        public UInt16 chan8_raw;
        /// <summary> Servo output port (set of 8 outputs = 1 port). Most MAVs will just use one, but this allows for more than 8 servos. </summary>
        public byte port;
        /// <summary> Receive signal strength indicator, 0: 0%, 100: 100%, 255: invalid/unknown. </summary>
        public byte rssi;

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#RC_CHANNELS_RAW";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.RC_CHANNELS_RAW;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_rc_channels_raw_t);
        }

        public RcChannelsRaw(MavLinkMessage message) : base(null)
        {
            if (message.messid.Equals(this.MessageID))
            {
                MAVLink.mavlink_rc_channels_raw_t data = (MAVLink.mavlink_rc_channels_raw_t)message.data_struct;
                this.chan1_raw = data.chan1_raw;
                this.chan2_raw = data.chan2_raw;
                this.chan3_raw = data.chan3_raw;
                this.chan4_raw = data.chan4_raw;
                this.chan5_raw = data.chan5_raw;
                this.chan6_raw = data.chan6_raw;
                this.chan7_raw = data.chan7_raw;
                this.chan8_raw = data.chan8_raw;
                this.port = data.port;
                this.rssi = data.rssi;
                this.time_boot_ms = data.time_boot_ms;
            }
        }
    }
}
