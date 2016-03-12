using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DroneManager.Models.MessageContainers
{
    /*
    POWER_STATUS ( #125 )

    Power supply status

    Field Name	Type	Description
    Vcc	uint16_t	5V rail voltage in millivolts
    Vservo	uint16_t	servo rail voltage in millivolts
    flags	uint16_t	power supply status flags (see MAV_POWER_STATUS enum)
    
    */

    public class PowerStatus : MessageContainerBase
    {
        /// <summary> 5V rail voltage in millivolts </summary>
        public UInt16 Vcc { get; set; }
        /// <summary> servo rail voltage in millivolts </summary>
        public UInt16 Vservo { get; set; }
        /// <summary> power supply status flags (see MAV_POWER_STATUS enum) </summary>
        //public UInt16 flags;
        public List<MAVLink.MAV_POWER_STATUS> flags { get; set; } = new List<MAVLink.MAV_POWER_STATUS>();

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#POWER_STATUS";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.POWER_STATUS;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_power_status_t);
        }

        public PowerStatus(MavLinkMessage message) : base(null)
        {
            if (message.messid.Equals(this.MessageID))
            {
                MAVLink.mavlink_power_status_t data = (MAVLink.mavlink_power_status_t)message.data_struct;
                this.Vcc = data.Vcc;
                this.Vservo = data.Vservo;

                // parse bit masks into lists
                IEnumerable<MAVLink.MAV_POWER_STATUS> values = EnumValues.GetValues<MAVLink.MAV_POWER_STATUS>();
                foreach (MAVLink.MAV_POWER_STATUS status in values)
                {
                    Boolean statusExists = Utilities.BitwiseOperations.bitExistsInValues((int)status, data.flags);
                    if (statusExists)
                    {
                        this.flags.Add(status);
                    }
                }
            }
        }
    }
}
