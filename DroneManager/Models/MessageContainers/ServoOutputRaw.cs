using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
    SERVO_OUTPUT_RAW ( #36 )

    The RAW values of the servo outputs (for RC input from the remote, use the RC_CHANNELS messages). The standard PPM modulation is as follows: 1000 microseconds: 0%, 2000 microseconds: 100%.

    Field Name	Type	Description
    time_usec	uint32_t	Timestamp (microseconds since system boot)
    port	uint8_t	Servo output port (set of 8 outputs = 1 port). Most MAVs will just use one, but this allows to encode more than 8 servos.
    servo1_raw	uint16_t	Servo output 1 value, in microseconds
    servo2_raw	uint16_t	Servo output 2 value, in microseconds
    servo3_raw	uint16_t	Servo output 3 value, in microseconds
    servo4_raw	uint16_t	Servo output 4 value, in microseconds
    servo5_raw	uint16_t	Servo output 5 value, in microseconds
    servo6_raw	uint16_t	Servo output 6 value, in microseconds
    servo7_raw	uint16_t	Servo output 7 value, in microseconds
    servo8_raw	uint16_t	Servo output 8 value, in microseconds
    */
    public class ServoOutputRaw : MessageContainerBase
    {
        /// <summary> Timestamp (microseconds since system boot) </summary>
        public UInt32 time_usec;
        /// <summary> Servo output 1 value, in microseconds </summary>
        public UInt16 servo1_raw;
        /// <summary> Servo output 2 value, in microseconds </summary>
        public UInt16 servo2_raw;
        /// <summary> Servo output 3 value, in microseconds </summary>
        public UInt16 servo3_raw;
        /// <summary> Servo output 4 value, in microseconds </summary>
        public UInt16 servo4_raw;
        /// <summary> Servo output 5 value, in microseconds </summary>
        public UInt16 servo5_raw;
        /// <summary> Servo output 6 value, in microseconds </summary>
        public UInt16 servo6_raw;
        /// <summary> Servo output 7 value, in microseconds </summary>
        public UInt16 servo7_raw;
        /// <summary> Servo output 8 value, in microseconds </summary>
        public UInt16 servo8_raw;
        /// <summary> Servo output port (set of 8 outputs = 1 port). Most MAVs will just use one, but this allows to encode more than 8 servos. </summary>
        public int port;

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#SERVO_OUTPUT_RAW";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.SERVO_OUTPUT_RAW;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_servo_output_raw_t);
        }

        public ServoOutputRaw(MavLinkMessage message) : base(null)
        {
            if (message.messid.Equals(this.MessageID))
            {
                MAVLink.mavlink_servo_output_raw_t data = (MAVLink.mavlink_servo_output_raw_t)message.data_struct;
                this.port = data.port;
                this.servo1_raw = data.servo1_raw;
                this.servo2_raw = data.servo2_raw;
                this.servo3_raw = data.servo3_raw;
                this.servo4_raw = data.servo4_raw;
                this.servo5_raw = data.servo5_raw;
                this.servo6_raw = data.servo6_raw;
                this.servo7_raw = data.servo7_raw;
                this.servo8_raw = data.servo8_raw;
                this.time_usec = data.time_usec;
            }
        }
    }
}
