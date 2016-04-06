using DroneManager.Models;
using DroneManager.Models.MessageContainers;
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
        public Guid id { get; }
        public String name { get; }
        public String port { get; }

        public ConnectionState state { get; }

        public HeartbeatDTO heartbeat_data { get; }

        public SystemStatusDTO sys_status_data { get; }

        public SystemTimeDTO sys_time_data { get; }

        public GpsRawIntDTO gps_raw_int { get; }

        public RawImuDTO raw_imu { get; }

        public ScaledPressureDTO scaled_pressure { get; }

        public AttitudeDTO attitude { get; }

        public GlobalPositionIntDTO global_position_int { get; }

        public RcChannelsRawDTO rc_channels_raw { get; }

        public ServoOutputRawDTO server_output_raw { get; }

        public MissionCurrentDTO mission_current { get; }

        public NavControllerOutputDTO nav_controller_output { get; }

        public TerrainReportDTO terrain_report { get; }

        public VfrHudDTO vfr_hud { get; }

        public ScaledImu2DTO scaled_imu_2 { get; }

        public PowerStatusDTO power_status { get; }

        public enum ConnectionState
        {
            CONNECTED,
            DISCONNECTED
        }

        public DroneDTO(Drone droneObj)
        {
            this.id = droneObj.id;
            this.name = droneObj.id.ToString();
            if (null != droneObj.connection)
            {
                this.port = droneObj.connection.port.PortName;
                if (droneObj.connection.port.IsOpen)
                {
                    this.state = ConnectionState.CONNECTED;
                }
                else
                {
                    this.state = ConnectionState.DISCONNECTED;
                }
            }
            this.heartbeat_data = new HeartbeatDTO(droneObj.getHearbeat());
            this.sys_status_data = new SystemStatusDTO(droneObj.getSystemStatus());
            this.sys_time_data = new SystemTimeDTO(droneObj.getSystemTime());
            this.gps_raw_int = new GpsRawIntDTO(droneObj.getGpsRawInt());
            this.raw_imu = new RawImuDTO(droneObj.getRawImu());
            this.scaled_pressure = new ScaledPressureDTO(droneObj.getScaledPressure());
            this.attitude = new AttitudeDTO(droneObj.getAttitude());
            this.global_position_int = new GlobalPositionIntDTO(droneObj.getGlobalPositionInt());
            this.rc_channels_raw = new RcChannelsRawDTO(droneObj.getRcChannelsRaw());
            this.server_output_raw = new ServoOutputRawDTO(droneObj.getServerOutputRaw());
            this.mission_current = new MissionCurrentDTO(droneObj.getMissionCurrent());
            this.nav_controller_output = new NavControllerOutputDTO(droneObj.getNavControllerOutput());
            this.vfr_hud = new VfrHudDTO(droneObj.getVfrHud());
            this.terrain_report = new TerrainReportDTO(droneObj.getTerrainReport());
            this.scaled_imu_2 = new ScaledImu2DTO(droneObj.getScaledImu2());
            this.power_status = new PowerStatusDTO(droneObj.getPowerStatus());
        }

    }
}