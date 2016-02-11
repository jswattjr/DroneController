using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
    GLOBAL_POSITION_INT ( #33 )

        The filtered global position (e.g. fused GPS and accelerometers). The position is in GPS-frame (right-handed, Z-up). It is designed as scaled integer message since the resolution of float is not sufficient.

        Field Name	Type	Description
        time_boot_ms	uint32_t	Timestamp (milliseconds since system boot)
        lat	int32_t	Latitude, expressed as degrees * 1E7
        lon	int32_t	Longitude, expressed as degrees * 1E7
        alt	int32_t	Altitude in meters, expressed as * 1000 (millimeters), AMSL (not WGS84 - note that virtually all GPS modules provide the AMSL as well)
        relative_alt	int32_t	Altitude above ground in meters, expressed as * 1000 (millimeters)
        vx	int16_t	Ground X Speed (Latitude, positive north), expressed as m/s * 100
        vy	int16_t	Ground Y Speed (Longitude, positive east), expressed as m/s * 100
        vz	int16_t	Ground Z Speed (Altitude, positive down), expressed as m/s * 100
        hdg	uint16_t	Vehicle heading (yaw angle) in degrees * 100, 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
    */
    public class GlobalPositionInt : MessageContainerBase
    {
        /// <summary> Timestamp (milliseconds since system boot) </summary>
        public UInt32 time_boot_ms;
        /// <summary> Latitude, expressed as * 1E7 </summary>
        public Int32 lat;
        /// <summary> Longitude, expressed as * 1E7 </summary>
        public Int32 lon;
        /// <summary> Altitude in meters, expressed as * 1000 (millimeters), AMSL (not WGS84 - note that virtually all GPS modules provide the AMSL as well) </summary>
        public Int32 alt;
        /// <summary> Altitude above ground in meters, expressed as * 1000 (millimeters) </summary>
        public Int32 relative_alt;
        /// <summary> Ground X Speed (Latitude), expressed as m/s * 100 </summary>
        public Int16 vx;
        /// <summary> Ground Y Speed (Longitude), expressed as m/s * 100 </summary>
        public Int16 vy;
        /// <summary> Ground Z Speed (Altitude), expressed as m/s * 100 </summary>
        public Int16 vz;
        /// <summary> Compass heading in degrees * 100, 0.0..359.99 degrees. If unknown, set to: UINT16_MAX </summary>
        public UInt16 hdg;

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#GLOBAL_POSITION_INT";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_global_position_int_t);
        }

        public GlobalPositionInt(MavLinkMessage message) : base(null)
        {
            if (message.messid.Equals(this.MessageID))
            {
                MAVLink.mavlink_global_position_int_t data = (MAVLink.mavlink_global_position_int_t)message.data_struct;
                this.alt = data.alt;
                this.hdg = data.hdg;
                this.lat = data.lat;
                this.lon = data.lon;
                this.relative_alt = data.relative_alt;
                this.time_boot_ms = data.time_boot_ms;
                this.vx = data.vx;
                this.vy = data.vy;
                this.vz = data.vz;
            }
        }
    }
}
