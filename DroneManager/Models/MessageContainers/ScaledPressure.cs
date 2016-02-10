using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
    SCALED_PRESSURE ( #29 )

    The pressure readings for the typical setup of one absolute and differential pressure sensor. The units are as specified in each field.

    Field Name	Type	Description
    time_boot_ms	uint32_t	Timestamp (milliseconds since system boot)
    press_abs	float	Absolute pressure (hectopascal)
    press_diff	float	Differential pressure 1 (hectopascal)
    temperature	int16_t	Temperature measurement (0.01 degrees celsius)
    */
    public class ScaledPressure : MessageContainerBase
    {
        public UInt32 time_boot_ms { get; set; }
        public Single press_abs { get; set; }
        public Single press_diff { get; set; }
        public Int16 temperature { get; set; }

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#SCALED_PRESSURE";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.SCALED_PRESSURE;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_scaled_pressure_t);
        }

        public ScaledPressure(MavLinkMessage message) : base(null)
        {
            if (message.messid == this.MessageID)
            {
                MAVLink.mavlink_scaled_pressure_t data = (MAVLink.mavlink_scaled_pressure_t)message.data_struct;
                this.press_abs = data.press_abs;
                this.press_diff = data.press_diff;
                this.temperature = data.temperature;
                this.time_boot_ms = data.time_boot_ms;
            }
        }
    }
}
