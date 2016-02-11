using DroneManager.Models.MessageContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneController.DataTransferObjects
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

    public class GlobalPositionIntDTO
    {
        /// <summary> Timestamp (milliseconds since system boot) </summary>
        public UInt32 time_boot_ms { get; set; }
        /// <summary> Latitude, expressed as * 1E7 </summary>
        public Int32 lat { get; set; }
        /// <summary> Longitude, expressed as * 1E7 </summary>
        public Int32 lon { get; set; }
        /// <summary> Altitude in meters, expressed as * 1000 (millimeters), AMSL (not WGS84 - note that virtually all GPS modules provide the AMSL as well) </summary>
        public Int32 alt { get; set; }
        /// <summary> Altitude above ground in meters, expressed as * 1000 (millimeters) </summary>
        public Int32 relative_alt { get; set; }
        /// <summary> Ground X Speed (Latitude), expressed as m/s * 100 </summary>
        public Int16 vx { get; set; }
        /// <summary> Ground Y Speed (Longitude), expressed as m/s * 100 </summary>
        public Int16 vy { get; set; }
        /// <summary> Ground Z Speed (Altitude), expressed as m/s * 100 </summary>
        public Int16 vz { get; set; }
        /// <summary> Compass heading in degrees * 100, 0.0..359.99 degrees. If unknown, set to: UINT16_MAX </summary>
        public UInt16 hdg { get; set; }

        public GlobalPositionIntDTO (GlobalPositionInt source)
        {
            Utilities.CopySimilar.CopyAll(source, this);
        }
    }
}