using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models.MessageContainers
{
    /*
    TERRAIN_REPORT ( #136 )

    Response from a TERRAIN_CHECK request

    Field Name	Type	Description
    lat	int32_t	Latitude (degrees *10^7)
    lon	int32_t	Longitude (degrees *10^7)
    spacing	uint16_t	grid spacing (zero if terrain at this location unavailable)
    terrain_height	float	Terrain height in meters AMSL
    current_height	float	Current vehicle height above lat/lon terrain height (meters)
    pending	uint16_t	Number of 4x4 terrain blocks waiting to be received or read from disk
    loaded	uint16_t	Number of 4x4 terrain blocks in memory

    */
    public class TerrainReport : MessageContainerBase
    {
        /// <summary> Latitude (degrees *10^7) </summary>
        public Int32 lat { get; set; }
        /// <summary> Longitude (degrees *10^7) </summary>
        public Int32 lon { get; set; }
        /// <summary> Terrain height in meters AMSL </summary>
        public Single terrain_height { get; set; }
        /// <summary> Current vehicle height above lat/lon terrain height (meters) </summary>
        public Single current_height { get; set; }
        /// <summary> grid spacing (zero if terrain at this location unavailable) </summary>
        public UInt16 spacing { get; set; }
        /// <summary> Number of 4x4 terrain blocks waiting to be received or read from disk </summary>
        public UInt16 pending { get; set; }
        /// <summary> Number of 4x4 terrain blocks in memory </summary>
        public UInt16 loaded { get; set; }

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#TERRAIN_REPORT";
            }
        }

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.TERRAIN_REPORT;
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_terrain_report_t);
        }

        public TerrainReport(MavLinkMessage message) : base(null)
        {
            if (message.messid.Equals(this.MessageID))
            {
                MAVLink.mavlink_terrain_report_t data = (MAVLink.mavlink_terrain_report_t)message.data_struct;
                this.current_height = data.current_height;
                this.lat = data.lat;
                this.loaded = data.loaded;
                this.lon = data.lon;
                this.pending = data.pending;
                this.spacing = data.spacing;
                this.terrain_height = data.terrain_height;
            }
        }
    }
}
