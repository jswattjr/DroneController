using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTransferObjects;
using DataTransferObjects.Commands;
using DataTransferObjects.Messages;
using DroneManager.Models;
using DroneManager.Models.MessageContainers;
using DroneParameterReference;

namespace DroneController.DTOFactory
{
    public static class DTOFactory
    {
        public static DroneDTO createDroneDTO(Drone droneObj)
        {
            DroneDTO result = new DroneDTO();
            result.id = droneObj.id;
            result.name = droneObj.id.ToString();
            if (null != droneObj.connection)
            {
                result.port = droneObj.connection.port.PortName;
                if (droneObj.connection.port.IsOpen)
                {
                    result.state = DroneDTO.ConnectionState.CONNECTED;
                }
                else
                {
                    result.state = DroneDTO.ConnectionState.DISCONNECTED;
                }
            }
            result.heartbeat_data = createHeartbeatDTO(droneObj.getHearbeat());
            result.sys_status_data = createSystemStatusDTO(droneObj.getSystemStatus());
            result.sys_time_data = createSystemTimeDTO(droneObj.getSystemTime());
            result.gps_raw_int = createGpsRawIntDTO(droneObj.getGpsRawInt());
            result.raw_imu = createRawImuDTO(droneObj.getRawImu());
            result.scaled_pressure = createScaledPressureDTO(droneObj.getScaledPressure());
            result.attitude = createAttitudeDTO(droneObj.getAttitude());
            result.global_position_int = createGlobalPositionIntDTO(droneObj.getGlobalPositionInt());
            result.rc_channels_raw = createRcChannelsRawDTO(droneObj.getRcChannelsRaw());
            result.server_output_raw = createServoOutputRawDTO(droneObj.getServerOutputRaw());
            result.mission_current = createMissionCurrentDTO(droneObj.getMissionCurrent());
            result.nav_controller_output = createNavControllerOutputDTO(droneObj.getNavControllerOutput());
            result.vfr_hud = createVfrHudDTO(droneObj.getVfrHud());
            result.terrain_report = createTerrainReportDTO(droneObj.getTerrainReport());
            result.scaled_imu_2 = createScaledImu2DTO(droneObj.getScaledImu2());
            result.power_status = createPowerStatusDTO(droneObj.getPowerStatus());
            return result;
        }

        public static AttitudeDTO createAttitudeDTO(Attitude source)
        {
            AttitudeDTO result = new AttitudeDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static CommandAckDTO createCommandAckDTO(CommandAck source)
        {
            CommandAckDTO result = new CommandAckDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static ParamValueDTO createParamValueDTO(ParamValue source)
        {
            ParamValueDTO result = new ParamValueDTO();
            Utilities.CopySimilar.CopyAll(source, result);

            // lookup static metadata about result parameter
            ParameterMetadata metadataLookup = new ParameterMetadata();
            ParameterMetadataEntry data = metadataLookup.fetchMetadata(source.param_id);
            if (null != data)
            {
                result.DisplayName = data.DisplayName;
                result.Description = data.Description;
                result.Units = data.Units;
                result.Upper = data.Upper;
                result.Lower = data.Lower;
            }

            return result;
        }

        public static HeartbeatDTO createHeartbeatDTO(Heartbeat data)
        {
            HeartbeatDTO result = new HeartbeatDTO();
            if (null != data)
            {
                result.type = data.type;
                result.autopilot = data.autopilot;
                result.custom_mode = data.custom_mode;
                result.base_mode = data.base_mode;
                result.system_status = data.system_status;
                result.mavlink_version = data.mavlink_version;
            }
            return result;
        }

        public static GlobalPositionIntDTO createGlobalPositionIntDTO(GlobalPositionInt source)
        {
            GlobalPositionIntDTO result = new GlobalPositionIntDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static GpsRawIntDTO createGpsRawIntDTO(GpsRawInt source)
        {
            GpsRawIntDTO result = new GpsRawIntDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static MissionCurrentDTO createMissionCurrentDTO(MissionCurrent source)
        {
            MissionCurrentDTO result = new MissionCurrentDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static NavControllerOutputDTO createNavControllerOutputDTO(NavControllerOutput source)
        {
            NavControllerOutputDTO result = new NavControllerOutputDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static ParametersDTO createParametersDTO(Dictionary<String, ParamValue> source)
        {
            ParametersDTO result = new ParametersDTO();
            result.parameters = new Dictionary<string, ParamValueDTO>();
            foreach (String key in source.Keys)
            {
                result.parameters.Add(key, createParamValueDTO(source[key]));
            }

            result.count = result.parameters.Keys.Count;
            return result;
        }

        public static PowerStatusDTO createPowerStatusDTO(PowerStatus source)
        {
            PowerStatusDTO result = new PowerStatusDTO();
            result.Vcc = source.Vcc;
            result.Vservo = source.Vservo;
            result.flags = new List<MAVLink.MAV_POWER_STATUS>(source.flags);
            return result;
        }

        public static RawImuDTO createRawImuDTO(RawImu source)
        {
            RawImuDTO result = new RawImuDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static RcChannelsRawDTO createRcChannelsRawDTO(RcChannelsRaw source)
        {
            RcChannelsRawDTO result = new RcChannelsRawDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static ScaledImu2DTO createScaledImu2DTO(ScaledImu2 source)
        {
            ScaledImu2DTO result = new ScaledImu2DTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static ScaledPressureDTO createScaledPressureDTO(ScaledPressure source)
        {
            ScaledPressureDTO result = new ScaledPressureDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static ServoOutputRawDTO createServoOutputRawDTO(ServoOutputRaw source)
        {
            ServoOutputRawDTO result = new ServoOutputRawDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static SystemStatusDTO createSystemStatusDTO(SystemStatus source)
        {
            SystemStatusDTO result = new SystemStatusDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            result.sensorsEnabled = new List<MAVLink.MAV_SYS_STATUS_SENSOR>(source.sensorsEnabled);
            result.sensorsHealth = new List<MAVLink.MAV_SYS_STATUS_SENSOR>(source.sensorsHealth);
            result.sensorsPresent = new List<MAVLink.MAV_SYS_STATUS_SENSOR>(source.sensorsPresent);
            return result;
        }

        public static SystemTimeDTO createSystemTimeDTO(SystemTime source)
        {
            SystemTimeDTO result = new SystemTimeDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static TerrainReportDTO createTerrainReportDTO(TerrainReport source)
        {
            TerrainReportDTO result = new TerrainReportDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static VfrHudDTO createVfrHudDTO(VfrHud source)
        {
            VfrHudDTO result = new VfrHudDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }


    }
}