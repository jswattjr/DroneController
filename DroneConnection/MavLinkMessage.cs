using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneConnection
{
    public class MavLinkMessage
    {
        // header values
        byte header { get; } // always 0xFE? Indicates start of message?
        int length { get; } // length of payload
        public int seq { get; } // packet sequence, can be examined against previous seq numbers to determine packet loss (number rolls from 0 through 255)
        public int sysid { get; } // ID of transmitter
        public int compid { get; } // ID of transmitter sub-component (currently identical to sysid)
        public MAVLink.MAVLINK_MSG_ID messid { get; } // message type ID

        // message body
        public object message; // this struct will be defined by messid

        // this constructor is used by the JsonConvert Deserialize code, DO NOT DELETE!
        public MavLinkMessage()
        {

        }

        public MavLinkMessage(byte[] buffer)
        {
            // fill out standard fields
            header = buffer[0];
            length = (int)buffer[1];
            seq = (int)buffer[2];
            sysid = (int)buffer[3];
            compid = (int)buffer[4];
            messid = (MAVLink.MAVLINK_MSG_ID)buffer[5];

            //create the object specified by the packet type
            object data = Activator.CreateInstance(getMessageType());
            // fill in the data of the object
            MavlinkUtil.ByteArrayToStructure(buffer, ref data, 6);
            message = data;
        }

        public Type getMessageType()
        {
            return MAVLink.MAVLINK_MESSAGE_INFO[(int)messid];
        }
    }
}
