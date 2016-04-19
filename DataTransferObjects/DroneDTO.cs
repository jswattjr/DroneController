using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTransferObjects.Commands;
using DataTransferObjects.Messages;

namespace DataTransferObjects
{
    public class DroneDTO
    {
        public Guid id { get; set; }
        public String name { get; set; }
        public String sysid { get; set; }
        public String componentid { get; set; }
        public String port { get; set; }

        public ConnectionState state { get; set; }

        public HeartbeatDTO heartbeat_data { get; set; }

        public SystemStatusDTO sys_status_data { get; set; }

        public SystemTimeDTO sys_time_data { get; set; }

        public GpsRawIntDTO gps_raw_int { get; set; }

        public RawImuDTO raw_imu { get; set; }

        public ScaledPressureDTO scaled_pressure { get; set; }

        public AttitudeDTO attitude { get; set; }

        public GlobalPositionIntDTO global_position_int { get; set; }

        public RcChannelsRawDTO rc_channels_raw { get; set; }

        public ServoOutputRawDTO server_output_raw { get; set; }

        public MissionCurrentDTO mission_current { get; set; }

        public NavControllerOutputDTO nav_controller_output { get; set; }

        public TerrainReportDTO terrain_report { get; set; }

        public VfrHudDTO vfr_hud { get; set; }

        public ScaledImu2DTO scaled_imu_2 { get; set; }

        public PowerStatusDTO power_status { get; set; }

        public enum ConnectionState
        {
            CONNECTED,
            DISCONNECTED
        }
    }
}