using DroneConnection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
    GPS_RAW_INT ( #24 )

The global position, as returned by the Global Positioning System (GPS). This is NOT the global position estimate of the system, but rather a RAW sensor value. See message GLOBAL_POSITION for the global position estimate. Coordinate frame is right-handed, Z-axis up (GPS frame).

    Field Name	Type	Description
    time_usec	uint64_t	Timestamp (microseconds since UNIX epoch or microseconds since system boot)
    fix_type	uint8_t	0-1: no fix, 2: 2D fix, 3: 3D fix, 4: DGPS, 5: RTK. Some applications will not use the value of this field unless it is at least two, so always correctly fill in the fix.
    lat	int32_t	Latitude (WGS84), in degrees * 1E7
    lon	int32_t	Longitude (WGS84), in degrees * 1E7
    alt	int32_t	Altitude (AMSL, NOT WGS84), in meters * 1000 (positive for up). Note that virtually all GPS modules provide the AMSL altitude in addition to the WGS84 altitude.
    eph	uint16_t	GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
    epv	uint16_t	GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
    vel	uint16_t	GPS ground speed (m/s * 100). If unknown, set to: UINT16_MAX
    cog	uint16_t	Course over ground (NOT heading, but direction of movement) in degrees * 100, 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
    satellites_visible	uint8_t	Number of satellites visible. If unknown, set to 255
    */
    public class GpsRawInt : MessageContainerBase
    {
        public UInt64 time_usec { get; set; }
        public UInt16 fix_type { get; set; }
        public Int32 lat { get; set; }
        public Int32 lon { get; set; }
        public Int32 alt { get; set; }
        public UInt16 eph { get; set; }
        public UInt16 epv { get; set; }
        public UInt16 vel { get; set; }
        public UInt16 cog { get; set; }
        public UInt16 satellites_visible { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FixType fixTypeEnum { get; set; }

        public enum FixType
        {
            NO_FIX_0 = 0,
            NO_FIX_1 = 1,
            FIX_2D = 2,
            FIX_3D = 3,
            FIX_DGPS = 4,
            FIX_RTK = 5
        }


        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#GPS_RAW_INT";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.GPS_RAW_INT;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_gps_raw_int_t);
        }

        public GpsRawInt(MavLinkMessage message) : base(null)
        {
            if (message.messid == this.MessageID)
            {
                MAVLink.mavlink_gps_raw_int_t raw_data = (MAVLink.mavlink_gps_raw_int_t)message.data_struct;
                this.alt = raw_data.alt;
                this.cog = raw_data.cog;
                this.eph = raw_data.eph;
                this.epv = raw_data.epv;
                this.fix_type = raw_data.fix_type;
                this.fixTypeEnum = (FixType)this.fix_type;
                this.lat = raw_data.lat;
                this.lon = raw_data.lon;
                this.satellites_visible = raw_data.satellites_visible;
                this.time_usec = raw_data.time_usec;
                this.vel = raw_data.vel;
            }
        }
    }
}
