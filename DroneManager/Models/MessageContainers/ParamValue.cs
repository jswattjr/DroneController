using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
    PARAM_VALUE ( #22 )

    Emit the value of a onboard parameter. The inclusion of param_count and param_index in the message allows the recipient to keep track of received parameters and allows him to re-request missing parameters after a loss or timeout.

    Field Name	Type	Description
    param_id	char[16]	Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string
    param_value	float	Onboard parameter value
    param_type	uint8_t	Onboard parameter type: see the MAV_PARAM_TYPE enum for supported data types.
    param_count	uint16_t	Total number of onboard parameters
    param_index	uint16_t	Index of this onboard parameter

    */
    public class ParamValue : MessageContainerBase
    {
        /// <summary> Onboard parameter value </summary>
        public Single param_value { get; set; }
        /// <summary> Total number of onboard parameters </summary>
        public UInt16 param_count { get; set; }
        /// <summary> Index of this onboard parameter </summary>
        public UInt16 param_index { get; set; }
        /// <summary> Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string </summary>
        public string param_id { get; set; }
        /// <summary> Onboard parameter type: see the MAV_PARAM_TYPE enum for supported data types. </summary>
        public MAVLink.MAV_PARAM_TYPE param_type { get; set; }

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#PARAM_VALUE";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.PARAM_VALUE;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_param_value_t);
        }

        public ParamValue(MavLinkMessage message) : base(null)
        {
            if (message.messid.Equals(this.MessageID))
            {
                MAVLink.mavlink_param_value_t data = (MAVLink.mavlink_param_value_t)message.data_struct;
                this.param_count = data.param_count;
                this.param_id = System.Text.ASCIIEncoding.ASCII.GetString(data.param_id);
                this.param_id = this.param_id.Replace("\0", string.Empty);
                this.param_index = data.param_index;
                this.param_type = (MAVLink.MAV_PARAM_TYPE)data.param_type;
                this.param_value = data.param_value;
            }
        }
    }
}
