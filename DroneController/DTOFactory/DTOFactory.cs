﻿using System;
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
            result.sysid = droneObj.connection.systemId.ToString();
            result.componentid = droneObj.connection.componentId.ToString();
            result.name = droneObj.id.ToString();
            if (null != droneObj.connection)
            {
                result.port = droneObj.connection.portName();
                if (droneObj.connection.isOpen())
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
            if (null == source)
            {
                return null;
            }
            AttitudeDTO result = new AttitudeDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static CommandAckDTO createCommandAckDTO(CommandAck source)
        {
            if (null == source)
            {
                return null;
            }
            CommandAckDTO result = new CommandAckDTO();
            result.command = source.command.ToString();
            result.result = source.result.ToString();
            return result;
        }

        public static ParamValueDTO createParamValueDTO(ParamValue source)
        {
            if (null == source)
            {
                return null;
            }
            ParamValueDTO result = new ParamValueDTO();

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
            result.param_type = source.param_type.ToString();
            result.param_value = source.param_value;
            result.param_index = source.param_index;
            result.param_id = source.param_id;
            result.param_count = source.param_count;

            return result;
        }

        public static HeartbeatDTO createHeartbeatDTO(Heartbeat data)
        {
            if (null == data)
            {
                return null;
            }
            HeartbeatDTO result = new HeartbeatDTO();
            if (null != data)
            {
                result.type = data.type.ToString();
                result.autopilot = data.autopilot.ToString();
                result.custom_mode = data.custom_mode;
                result.base_mode = new List<string>();
                foreach(MAVLink.MAV_MODE_FLAG flag in data.base_mode)
                {
                    result.base_mode.Add(flag.ToString());
                }
                result.system_status = data.system_status.ToString();
                result.mavlink_version = data.mavlink_version;
            }
            return result;
        }

        public static GlobalPositionIntDTO createGlobalPositionIntDTO(GlobalPositionInt source)
        {
            if (null == source)
            {
                return null;
            }
            GlobalPositionIntDTO result = new GlobalPositionIntDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static GpsRawIntDTO createGpsRawIntDTO(GpsRawInt source)
        {
            if (null == source)
            {
                return null;
            }
            GpsRawIntDTO result = new GpsRawIntDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            result.fixTypeLabel = source.fixTypeEnum.ToString();
            return result;
        }

        public static MissionCurrentDTO createMissionCurrentDTO(MissionCurrent source)
        {
            if (null == source)
            {
                return null;
            }
            MissionCurrentDTO result = new MissionCurrentDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static NavControllerOutputDTO createNavControllerOutputDTO(NavControllerOutput source)
        {
            if (null == source)
            {
                return null;
            }
            NavControllerOutputDTO result = new NavControllerOutputDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static ParametersDTO createParametersDTO(Dictionary<String, ParamValue> source)
        {
            if (null == source)
            {
                return null;
            }
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
            if (null == source)
            {
                return null;
            }
            PowerStatusDTO result = new PowerStatusDTO();
            result.Vcc = source.Vcc;
            result.Vservo = source.Vservo;
            result.flags = new List<String>();
            foreach (MAVLink.MAV_POWER_STATUS flag in source.flags)
            {
                result.flags.Add(flag.ToString());
            }
            return result;
        }

        public static RawImuDTO createRawImuDTO(RawImu source)
        {
            if (null == source)
            {
                return null;
            }
            RawImuDTO result = new RawImuDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static RcChannelsRawDTO createRcChannelsRawDTO(RcChannelsRaw source)
        {
            if (null == source)
            {
                return null;
            }
            RcChannelsRawDTO result = new RcChannelsRawDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static ScaledImu2DTO createScaledImu2DTO(ScaledImu2 source)
        {
            if (null == source)
            {
                return null;
            }
            ScaledImu2DTO result = new ScaledImu2DTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static ScaledPressureDTO createScaledPressureDTO(ScaledPressure source)
        {
            if (null == source)
            {
                return null;
            }
            ScaledPressureDTO result = new ScaledPressureDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static ServoOutputRawDTO createServoOutputRawDTO(ServoOutputRaw source)
        {
            if (null == source)
            {
                return null;
            }
            ServoOutputRawDTO result = new ServoOutputRawDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static SystemStatusDTO createSystemStatusDTO(SystemStatus source)
        {
            if (null == source)
            {
                return null;
            }
            SystemStatusDTO result = new SystemStatusDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            result.sensorsEnabled = new List<String>();
            foreach (MAVLink.MAV_SYS_STATUS_SENSOR flag in source.sensorsEnabled)
            {
                result.sensorsEnabled.Add(flag.ToString());
            }
            result.sensorsHealth = new List<String>();
            foreach (MAVLink.MAV_SYS_STATUS_SENSOR flag in source.sensorsHealth)
            {
                result.sensorsHealth.Add(flag.ToString());
            }
            result.sensorsPresent = new List<String>();
            foreach(MAVLink.MAV_SYS_STATUS_SENSOR flag in source.sensorsPresent)
            {
                result.sensorsPresent.Add(flag.ToString());
            }
            return result;
        }

        public static SystemTimeDTO createSystemTimeDTO(SystemTime source)
        {
            if (null == source)
            {
                return null;
            }
            SystemTimeDTO result = new SystemTimeDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static TerrainReportDTO createTerrainReportDTO(TerrainReport source)
        {
            if (null == source)
            {
                return null;
            }
            TerrainReportDTO result = new TerrainReportDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }

        public static VfrHudDTO createVfrHudDTO(VfrHud source)
        {
            if (null == source)
            {
                return null;
            }
            VfrHudDTO result = new VfrHudDTO();
            Utilities.CopySimilar.CopyAll(source, result);
            return result;
        }


    }
}