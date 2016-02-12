using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
    MISSION_CURRENT ( #42 )

        Message that announces the sequence number of the current active mission item. The MAV will fly towards this mission item.

        Field Name	Type	Description
        seq	uint16_t	Sequence
       */
    public class MissionCurrent : MessageContainerBase
    {
        /// <summary> Sequence </summary>
        public UInt16 seq;

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#MISSION_CURRENT";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.MISSION_CURRENT;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_mission_current_t);
        }

        public MissionCurrent(MavLinkMessage message) : base(null)
        {
            if (message.messid.Equals(this.MessageID))
            {
                MAVLink.mavlink_mission_current_t data = (MAVLink.mavlink_mission_current_t)message.data_struct;
                this.seq = data.seq;
            }
        }
    }
}
