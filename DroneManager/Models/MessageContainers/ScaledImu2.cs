using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
    SCALED_IMU2 ( #116 )

    The RAW IMU readings for secondary 9DOF sensor setup. This message should contain the scaled values to the described units

    Field Name	Type	Description
    time_boot_ms	uint32_t	Timestamp (milliseconds since system boot)
    xacc	int16_t	X acceleration (mg)
    yacc	int16_t	Y acceleration (mg)
    zacc	int16_t	Z acceleration (mg)
    xgyro	int16_t	Angular speed around X axis (millirad /sec)
    ygyro	int16_t	Angular speed around Y axis (millirad /sec)
    zgyro	int16_t	Angular speed around Z axis (millirad /sec)
    xmag	int16_t	X Magnetic field (milli tesla)
    ymag	int16_t	Y Magnetic field (milli tesla)
    zmag	int16_t	Z Magnetic field (milli tesla)

    */
    public class ScaledImu2 : MessageContainerBase
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
        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#SCALED_IMU2";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.SCALED_IMU2;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_scaled_imu2_t);
        }

        public ScaledImu2(MavLinkMessage message) : base(null)
        {
            if (message.messid.Equals(this.MessageID))
            {
                MAVLink.mavlink_scaled_imu2_t data = (MAVLink.mavlink_scaled_imu2_t)message.data_struct;
                this.time_boot_ms = data.time_boot_ms;
                this.xacc = data.xacc;
                this.xgyro = data.xgyro;
                this.xmag = data.xmag;
                this.yacc = data.yacc;
                this.ygyro = data.ygyro;
                this.ymag = data.ymag;
                this.zacc = data.zacc;
                this.zgyro = data.zgyro;
                this.zmag = data.zmag;
            }
        }
    }
}
