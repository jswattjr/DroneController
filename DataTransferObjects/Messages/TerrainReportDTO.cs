using DroneManager.Models.MessageContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class TerrainReportDTO
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

        public TerrainReportDTO (TerrainReport source)
        {
            Utilities.CopySimilar.CopyAll(source, this);
        }
    }
}