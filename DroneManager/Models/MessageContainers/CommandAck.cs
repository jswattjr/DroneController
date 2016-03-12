using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
     COMMAND_ACK ( #77 )

    Report status of a command. Includes feedback wether the command was executed.

    Field Name	Type	Description
    command	uint16_t	Command ID, as defined by MAV_CMD enum.
    result	uint8_t	See MAV_RESULT enum
    */
    public class CommandAck : MessageContainerBase
    {
        /// <summary> Command ID, as defined by MAV_CMD enum. </summary>
        public MAVLink.MAV_CMD command { get; set; }
        /// <summary> See MAV_RESULT enum </summary>
        public MAVLink.MAV_RESULT result { get; set; }

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#COMMAND_ACK";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.COMMAND_ACK;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_command_ack_t);
        }

        public CommandAck(MavLinkMessage message) : base(null)
        {
            if (message.messid.Equals(this.MessageID))
            {
                MAVLink.mavlink_command_ack_t data = (MAVLink.mavlink_command_ack_t)message.data_struct;
                this.command = (MAVLink.MAV_CMD)data.command;
                this.result = (MAVLink.MAV_RESULT)data.result;
            }
        }
    }
}
