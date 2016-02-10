using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
 The RAW IMU readings for the usual 9DOF sensor setup. This message should always contain the true raw values without any scaling to allow data capture and system debugging.

     Field Name	Type	Description
     time_usec	uint64_t	Timestamp (microseconds since UNIX epoch or microseconds since system boot)
     xacc	int16_t	X acceleration (raw)
     yacc	int16_t	Y acceleration (raw)
     zacc	int16_t	Z acceleration (raw)
     xgyro	int16_t	Angular speed around X axis (raw)
     ygyro	int16_t	Angular speed around Y axis (raw)
     zgyro	int16_t	Angular speed around Z axis (raw)
     xmag	int16_t	X Magnetic field (raw)
     ymag	int16_t	Y Magnetic field (raw)
     zmag	int16_t	Z Magnetic field (raw)
     */
    public class RawImu : MessageContainerBase
    {
        public Int16 xacc { get; set; }
        public Int16 yacc { get; set; }
        public Int16 zacc { get; set; }
        public Int16 xgyro { get; set; }
        public Int16 ygyro { get; set; }
        public Int16 zgyro { get; set; }
        public Int16 xmag { get; set; }
        public Int16 ymag { get; set; }
        public Int16 zmag { get; set; }
 
        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#RAW_IMU";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.RAW_IMU;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_raw_imu_t);
        }

        public RawImu(MavLinkMessage message) : base (null)
        {
            if (message.messid == this.MessageID)
            {
                MAVLink.mavlink_raw_imu_t raw_data = (MAVLink.mavlink_raw_imu_t)message.data_struct;
                this.xacc = raw_data.xacc;
                this.yacc = raw_data.yacc;
                this.zacc = raw_data.zacc;
                this.xgyro = raw_data.xgyro;
                this.ygyro = raw_data.ygyro;
                this.zgyro = raw_data.zgyro;
                this.xmag = raw_data.xmag;
                this.ymag = raw_data.ymag;
                this.zmag = raw_data.zmag;
            }
        }
    }
}
