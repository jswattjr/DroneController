using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneConnection;
using Newtonsoft.Json;
using DroneManager.Models.MessageContainers;
using DroneController.DataTransferObjects;

namespace Tests
{
    [TestClass]
    public class UnitTests
    {
        MavLinkMessage createSampleMessage()
        {
            MAVLink.mavlink_heartbeat_t sampleStruct = new MAVLink.mavlink_heartbeat_t();
            sampleStruct.autopilot = 1;
            sampleStruct.base_mode = 2;
            sampleStruct.custom_mode = 3;
            sampleStruct.mavlink_version = 4;
            sampleStruct.system_status = 5;
            sampleStruct.type = 6;

            MavLinkMessage sampleMessage = createSampleMessage(MAVLink.MAVLINK_MSG_ID.HEARTBEAT, sampleStruct);

            return sampleMessage;
        }

        MavLinkMessage createSampleMessage(MAVLink.MAVLINK_MSG_ID id, object data_struct)
        {
            MavLinkMessage sampleMessage = new MavLinkMessage();
            sampleMessage.messid = id;
            sampleMessage.seq = 128;
            sampleMessage.sysid = 12;
            sampleMessage.compid = 12;
            sampleMessage.data_struct = data_struct;
            return sampleMessage;
        }

        [TestMethod]
        public void CheckMavLinkMessageSerialization()
        {
            MavLinkMessage sampleMessage = createSampleMessage();
            MAVLink.mavlink_heartbeat_t sampleStruct = (MAVLink.mavlink_heartbeat_t)sampleMessage.data_struct;

            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            String serialized = JsonConvert.SerializeObject(sampleMessage, settings);

            MavLinkMessage deserialized = JsonConvert.DeserializeObject<MavLinkMessage>(serialized, settings);
            MAVLink.mavlink_heartbeat_t dstruct = (MAVLink.mavlink_heartbeat_t)deserialized.data_struct;

            Assert.AreEqual(sampleMessage.compid, deserialized.compid);
            Assert.AreEqual(sampleMessage.sysid, deserialized.sysid);
            Assert.AreEqual(sampleMessage.seq, deserialized.seq);
            Assert.AreEqual(sampleMessage.messid, deserialized.messid);

            Assert.AreEqual(sampleStruct.autopilot, dstruct.autopilot);
            Assert.AreEqual(sampleStruct.base_mode, dstruct.base_mode);
            Assert.AreEqual(sampleStruct.custom_mode, dstruct.custom_mode);
            Assert.AreEqual(sampleStruct.mavlink_version, dstruct.mavlink_version);
            Assert.AreEqual(sampleStruct.system_status, dstruct.system_status);
            Assert.AreEqual(sampleStruct.type, dstruct.type);


        }

        [TestMethod]
        public void CheckHeartbeatObject()
        {
            MavLinkMessage message = createSampleMessage();
            MAVLink.mavlink_heartbeat_t sampleStruct = (MAVLink.mavlink_heartbeat_t)message.data_struct;

            Heartbeat heartbeatContainer = new Heartbeat(message);

            Assert.AreEqual((MAVLink.MAV_AUTOPILOT)sampleStruct.autopilot, heartbeatContainer.autopilot);
            Assert.AreEqual((MAVLink.MAV_MODE_FLAG)sampleStruct.base_mode, heartbeatContainer.base_mode);
            Assert.AreEqual(sampleStruct.custom_mode, heartbeatContainer.custom_mode);
            Assert.AreEqual(sampleStruct.mavlink_version, heartbeatContainer.mavlink_version);
            Assert.AreEqual((MAVLink.MAV_STATE)sampleStruct.system_status, heartbeatContainer.system_status);
            Assert.AreEqual((MAVLink.MAV_TYPE)sampleStruct.type, heartbeatContainer.type);

            HeartbeatDTO dto = new HeartbeatDTO(heartbeatContainer);

            Assert.AreEqual(dto.autopilot, heartbeatContainer.autopilot);
            Assert.AreEqual(dto.base_mode, heartbeatContainer.base_mode);
            Assert.AreEqual(dto.custom_mode, heartbeatContainer.custom_mode);
            Assert.AreEqual(dto.mavlink_version, heartbeatContainer.mavlink_version);
            Assert.AreEqual(dto.system_status, heartbeatContainer.system_status);
            Assert.AreEqual(dto.type, heartbeatContainer.type);

            String json = JsonConvert.SerializeObject(dto);
        }

        [TestMethod]
        public void CopySimilarStressTest()
        {
            // 0.644s with direct copy
            // 8s with reflection
            int testsize = 1000000;
            for (int count = 0; count < testsize; count++)
            {
                CheckHeartbeatObject();
            }
        }

        [TestMethod]
        public void CheckSystemStatusObject()
        {
            MAVLink.mavlink_sys_status_t statusStruct = new MAVLink.mavlink_sys_status_t();
            statusStruct.voltage_battery = 1;
            statusStruct.current_battery = 2;
            statusStruct.battery_remaining = 3;
            statusStruct.drop_rate_comm = 4;
            statusStruct.errors_comm = 5;
            statusStruct.errors_count1 = 6;
            statusStruct.errors_count2 = 7;
            statusStruct.errors_count3 = 8;
            statusStruct.errors_count4 = 9;

            MavLinkMessage message = new MavLinkMessage();
            message.compid = 1;
            message.messid = MAVLink.MAVLINK_MSG_ID.SYS_STATUS;
            message.seq = 1;
            message.sysid = 1;
            message.data_struct = statusStruct;

            SystemStatus systemStatus = new SystemStatus(message);

            Assert.AreEqual(statusStruct.voltage_battery, systemStatus.voltage_battery);
            Assert.AreEqual(statusStruct.current_battery, systemStatus.current_battery);
            Assert.AreEqual(statusStruct.battery_remaining, systemStatus.battery_remaining);
            Assert.AreEqual(statusStruct.drop_rate_comm, systemStatus.drop_rate_comm);
            Assert.AreEqual(statusStruct.errors_comm, systemStatus.errors_comm);
            Assert.AreEqual(statusStruct.errors_count1, systemStatus.errors_count1);
            Assert.AreEqual(statusStruct.errors_count2, systemStatus.errors_count2);
            Assert.AreEqual(statusStruct.errors_count3, systemStatus.errors_count3);
            Assert.AreEqual(statusStruct.errors_count4, systemStatus.errors_count4);

            SystemStatusDTO dto = new SystemStatusDTO(systemStatus);

            Assert.AreEqual(dto.voltage_battery, systemStatus.voltage_battery);
            Assert.AreEqual(dto.current_battery, systemStatus.current_battery);
            Assert.AreEqual(dto.battery_remaining, systemStatus.battery_remaining);
            Assert.AreEqual(dto.drop_rate_comm, systemStatus.drop_rate_comm);
            Assert.AreEqual(dto.errors_comm, systemStatus.errors_comm);
            Assert.AreEqual(dto.errors_count1, systemStatus.errors_count1);
            Assert.AreEqual(dto.errors_count2, systemStatus.errors_count2);
            Assert.AreEqual(dto.errors_count3, systemStatus.errors_count3);
            Assert.AreEqual(dto.errors_count4, systemStatus.errors_count4);

            String json = JsonConvert.SerializeObject(dto);
        }

        [TestMethod]
        public void CheckSystemTimeObject()
        {
            MAVLink.mavlink_system_time_t timeStruct = new MAVLink.mavlink_system_time_t();
            timeStruct.time_boot_ms = 1;
            timeStruct.time_unix_usec = 2;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.SYSTEM_TIME, timeStruct);

            SystemTime systemTime = new SystemTime(message);

            Assert.AreEqual(timeStruct.time_boot_ms, systemTime.time_boot_ms);
            Assert.AreEqual(timeStruct.time_unix_usec, systemTime.time_unix_sec);

            SystemTimeDTO dto = new SystemTimeDTO(systemTime);

            Assert.AreEqual(dto.time_boot_ms, systemTime.time_boot_ms);
            Assert.AreEqual(dto.time_unix_sec, systemTime.time_unix_sec);

            String json = JsonConvert.SerializeObject(dto);
        }

        [TestMethod]
        public void CheckGpsRawIntObject()
        {
            MAVLink.mavlink_gps_raw_int_t gpsStruct = new MAVLink.mavlink_gps_raw_int_t();
            gpsStruct.alt = 1;
            gpsStruct.cog = 2;
            gpsStruct.eph = 3;
            gpsStruct.epv = 4;
            gpsStruct.fix_type = 3;
            gpsStruct.lat = 6;
            gpsStruct.lon = 7;
            gpsStruct.satellites_visible = 8;
            gpsStruct.time_usec = 9;
            gpsStruct.vel = 10;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.GPS_RAW_INT, gpsStruct);

            GpsRawInt obj = new GpsRawInt(message);

            Assert.AreEqual(gpsStruct.alt, obj.alt);
            Assert.AreEqual(gpsStruct.cog, obj.cog);
            Assert.AreEqual(gpsStruct.eph, obj.eph);
            Assert.AreEqual(gpsStruct.epv, obj.epv);
            Assert.AreEqual(gpsStruct.fix_type, obj.fix_type);
            Assert.AreEqual(gpsStruct.lat, obj.lat);
            Assert.AreEqual(gpsStruct.lon, obj.lon);
            Assert.AreEqual(gpsStruct.satellites_visible, obj.satellites_visible);
            Assert.AreEqual(gpsStruct.time_usec, obj.time_usec);
            Assert.AreEqual(gpsStruct.vel, obj.vel);
            Assert.AreEqual(GpsRawInt.FixType.FIX_3D, obj.fixTypeEnum);

            GpsRawIntDTO dto = new GpsRawIntDTO(obj);

            Assert.AreEqual(dto.alt, obj.alt);
            Assert.AreEqual(dto.cog, obj.cog);
            Assert.AreEqual(dto.eph, obj.eph);
            Assert.AreEqual(dto.epv, obj.epv);
            Assert.AreEqual(dto.fix_type, obj.fix_type);
            Assert.AreEqual(dto.lat, obj.lat);
            Assert.AreEqual(dto.lon, obj.lon);
            Assert.AreEqual(dto.satellites_visible, obj.satellites_visible);
            Assert.AreEqual(dto.time_usec, obj.time_usec);
            Assert.AreEqual(dto.vel, obj.vel);
            Assert.AreEqual(GpsRawInt.FixType.FIX_3D, obj.fixTypeEnum);

        }

        [TestMethod]
        public void CheckRawImuObject()
        {
            MAVLink.mavlink_raw_imu_t data = new MAVLink.mavlink_raw_imu_t();
            data.xacc = 1;
            data.xgyro = 2;
            data.xmag = 3;
            data.yacc = 4;
            data.ygyro = 5;
            data.ymag = 6;
            data.zacc = 7;
            data.zgyro = 8;
            data.zmag = 9;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.RAW_IMU, data);

            RawImu obj = new RawImu(message);

            Assert.AreEqual(data.xacc, obj.xacc);
            Assert.AreEqual(data.yacc, obj.yacc);
            Assert.AreEqual(data.zacc, obj.zacc);
            Assert.AreEqual(data.xgyro, obj.xgyro);
            Assert.AreEqual(data.ygyro, obj.ygyro);
            Assert.AreEqual(data.zgyro, obj.zgyro);
            Assert.AreEqual(data.xmag, obj.xmag);
            Assert.AreEqual(data.ymag, obj.ymag);
            Assert.AreEqual(data.zmag, obj.zmag);

            RawImuDTO dto = new RawImuDTO(obj);
            Assert.AreEqual(dto.xacc, obj.xacc);
            Assert.AreEqual(dto.yacc, obj.yacc);
            Assert.AreEqual(dto.zacc, obj.zacc);
            Assert.AreEqual(dto.xgyro, obj.xgyro);
            Assert.AreEqual(dto.ygyro, obj.ygyro);
            Assert.AreEqual(dto.zgyro, obj.zgyro);
            Assert.AreEqual(dto.xmag, obj.xmag);
            Assert.AreEqual(dto.ymag, obj.ymag);
            Assert.AreEqual(dto.zmag, obj.zmag);

        }

        [TestMethod]
        public void CheckScaledPressureObject()
        {
            MAVLink.mavlink_scaled_pressure_t data = new MAVLink.mavlink_scaled_pressure_t();
            data.press_abs = 1.0f;
            data.press_diff = 2.0f;
            data.temperature = 3;
            data.time_boot_ms = 4;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.SCALED_PRESSURE, data);

            ScaledPressure obj = new ScaledPressure(message);

            Assert.AreEqual(data.press_abs, obj.press_abs);
            Assert.AreEqual(data.press_diff, obj.press_diff);
            Assert.AreEqual(data.temperature, obj.temperature);
            Assert.AreEqual(data.time_boot_ms, obj.time_boot_ms);

            ScaledPressureDTO dto = new ScaledPressureDTO(obj);

            Assert.AreEqual(dto.press_abs, obj.press_abs);
            Assert.AreEqual(dto.press_diff, obj.press_diff);
            Assert.AreEqual(dto.temperature, obj.temperature);
            Assert.AreEqual(dto.time_boot_ms, obj.time_boot_ms);


        }

        [TestMethod]
        public void CheckAttitudeObject()
        {
            MAVLink.mavlink_attitude_t data = new MAVLink.mavlink_attitude_t();
            data.pitch = 1;
            data.pitchspeed = 2;
            data.roll = 3;
            data.rollspeed = 4;
            data.time_boot_ms = 5;
            data.yaw = 6;
            data.yawspeed = 7;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.ATTITUDE, data);

            Attitude obj = new Attitude(message);

            Assert.AreEqual(data.pitch, obj.pitch);
            Assert.AreEqual(data.pitchspeed, obj.pitchspeed);
            Assert.AreEqual(data.roll, obj.roll);
            Assert.AreEqual(data.rollspeed, obj.rollspeed);
            Assert.AreEqual(data.time_boot_ms, obj.time_boot_ms);
            Assert.AreEqual(data.yaw, obj.yaw);
            Assert.AreEqual(data.yawspeed, obj.yawspeed);

            AttitudeDTO dto = new AttitudeDTO(obj);

            Assert.AreEqual(dto.pitch, obj.pitch);
            Assert.AreEqual(dto.pitchspeed, obj.pitchspeed);
            Assert.AreEqual(dto.roll, obj.roll);
            Assert.AreEqual(dto.rollspeed, obj.rollspeed);
            Assert.AreEqual(dto.time_boot_ms, obj.time_boot_ms);
            Assert.AreEqual(dto.yaw, obj.yaw);
            Assert.AreEqual(dto.yawspeed, obj.yawspeed);

        }

        [TestMethod]
        public void CheckGlobalPostionIntObj()
        {
            MAVLink.mavlink_global_position_int_t data = new MAVLink.mavlink_global_position_int_t();
            data.alt = 1;
            data.hdg = 2;
            data.lat = 3;
            data.lon = 4;
            data.relative_alt = 5;
            data.time_boot_ms = 6;
            data.vx = 7;
            data.vy = 8;
            data.vz = 9;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT, data);

            GlobalPositionInt obj = new GlobalPositionInt(message);

            Assert.AreEqual(data.alt, obj.alt);
            Assert.AreEqual(data.hdg, obj.hdg);
            Assert.AreEqual(data.lat, obj.lat);
            Assert.AreEqual(data.lon, obj.lon);
            Assert.AreEqual(data.relative_alt, obj.relative_alt);
            Assert.AreEqual(data.time_boot_ms, obj.time_boot_ms);
            Assert.AreEqual(data.vx, obj.vx);
            Assert.AreEqual(data.vy, obj.vy);
            Assert.AreEqual(data.vz, obj.vz);

            GlobalPositionIntDTO dto = new GlobalPositionIntDTO(obj);

            Assert.AreEqual(dto.alt, obj.alt);
            Assert.AreEqual(dto.hdg, obj.hdg);
            Assert.AreEqual(dto.lat, obj.lat);
            Assert.AreEqual(dto.lon, obj.lon);
            Assert.AreEqual(dto.relative_alt, obj.relative_alt);
            Assert.AreEqual(dto.time_boot_ms, obj.time_boot_ms);
            Assert.AreEqual(dto.vx, obj.vx);
            Assert.AreEqual(dto.vy, obj.vy);
            Assert.AreEqual(dto.vz, obj.vz);

        }

        [TestMethod]
        public void CheckRcChannelsRawObject()
        {
            MAVLink.mavlink_rc_channels_raw_t data = new MAVLink.mavlink_rc_channels_raw_t();
            data.chan1_raw = 1;
            data.chan2_raw = 2;
            data.chan3_raw = 3;
            data.chan4_raw = 4;
            data.chan5_raw = 5;
            data.chan6_raw = 6;
            data.chan7_raw = 7;
            data.chan8_raw = 8;
            data.port = 9;
            data.rssi = 10;
            data.time_boot_ms = 11;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.RC_CHANNELS_RAW, data);

            RcChannelsRaw obj = new RcChannelsRaw(message);

            Assert.AreEqual(data.chan1_raw, obj.chan1_raw);
            Assert.AreEqual(data.chan2_raw, obj.chan2_raw);
            Assert.AreEqual(data.chan3_raw, obj.chan3_raw);
            Assert.AreEqual(data.chan4_raw, obj.chan4_raw);
            Assert.AreEqual(data.chan5_raw, obj.chan5_raw);
            Assert.AreEqual(data.chan6_raw, obj.chan6_raw);
            Assert.AreEqual(data.chan7_raw, obj.chan7_raw);
            Assert.AreEqual(data.chan8_raw, obj.chan8_raw);
            Assert.AreEqual(data.port, obj.port);
            Assert.AreEqual(data.rssi, obj.rssi);
            Assert.AreEqual(data.time_boot_ms, obj.time_boot_ms);

            RcChannelsRawDTO dto = new RcChannelsRawDTO(obj);

            Assert.AreEqual(dto.chan1_raw, obj.chan1_raw);
            Assert.AreEqual(dto.chan2_raw, obj.chan2_raw);
            Assert.AreEqual(dto.chan3_raw, obj.chan3_raw);
            Assert.AreEqual(dto.chan4_raw, obj.chan4_raw);
            Assert.AreEqual(dto.chan5_raw, obj.chan5_raw);
            Assert.AreEqual(dto.chan6_raw, obj.chan6_raw);
            Assert.AreEqual(dto.chan7_raw, obj.chan7_raw);
            Assert.AreEqual(dto.chan8_raw, obj.chan8_raw);
            Assert.AreEqual(dto.port, obj.port);
            Assert.AreEqual(dto.rssi, obj.rssi);
            Assert.AreEqual(dto.time_boot_ms, obj.time_boot_ms);

        }

        [TestMethod]
        public void CheckServoOutputRawObject()
        {
            MAVLink.mavlink_servo_output_raw_t data = new MAVLink.mavlink_servo_output_raw_t();
            data.port = 1;
            data.servo1_raw = 2;
            data.servo2_raw = 3;
            data.servo3_raw = 4;
            data.servo4_raw = 5;
            data.servo5_raw = 6;
            data.servo6_raw = 7;
            data.servo7_raw = 8;
            data.servo8_raw = 9;
            data.time_usec = 10;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.SERVO_OUTPUT_RAW, data);

            ServoOutputRaw obj = new ServoOutputRaw(message);

            Assert.AreEqual(data.port, obj.port);
            Assert.AreEqual(data.servo1_raw, obj.servo1_raw);
            Assert.AreEqual(data.servo2_raw, obj.servo2_raw);
            Assert.AreEqual(data.servo3_raw, obj.servo3_raw);
            Assert.AreEqual(data.servo4_raw, obj.servo4_raw);
            Assert.AreEqual(data.servo5_raw, obj.servo5_raw);
            Assert.AreEqual(data.servo6_raw, obj.servo6_raw);
            Assert.AreEqual(data.servo7_raw, obj.servo7_raw);
            Assert.AreEqual(data.servo8_raw, obj.servo8_raw);
            Assert.AreEqual(data.time_usec, obj.time_usec);

            ServoOutputRawDTO dto = new ServoOutputRawDTO(obj);

            Assert.AreEqual(dto.port, obj.port);
            Assert.AreEqual(dto.servo1_raw, obj.servo1_raw);
            Assert.AreEqual(dto.servo2_raw, obj.servo2_raw);
            Assert.AreEqual(dto.servo3_raw, obj.servo3_raw);
            Assert.AreEqual(dto.servo4_raw, obj.servo4_raw);
            Assert.AreEqual(dto.servo5_raw, obj.servo5_raw);
            Assert.AreEqual(dto.servo6_raw, obj.servo6_raw);
            Assert.AreEqual(dto.servo7_raw, obj.servo7_raw);
            Assert.AreEqual(dto.servo8_raw, obj.servo8_raw);
            Assert.AreEqual(dto.time_usec, obj.time_usec);

        }

        [TestMethod]
        public void CheckMissionCurrentObject()
        {
            MAVLink.mavlink_mission_current_t data = new MAVLink.mavlink_mission_current_t();
            data.seq = 7;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.MISSION_CURRENT, data);

            MissionCurrent obj = new MissionCurrent(message);

            Assert.AreEqual(data.seq, obj.seq);

            MissionCurrentDTO dto = new MissionCurrentDTO(obj);

            Assert.AreEqual(dto.seq, obj.seq);
        }

        [TestMethod]
        public void CheckNavControllerOutput()
        {
            MAVLink.mavlink_nav_controller_output_t data = new MAVLink.mavlink_nav_controller_output_t();
            data.alt_error = 1;
            data.aspd_error = 2;
            data.nav_bearing = 2;
            data.nav_pitch = 3;
            data.nav_roll = 4;
            data.target_bearing = 5;
            data.wp_dist = 6;
            data.xtrack_error = 7;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.NAV_CONTROLLER_OUTPUT, data);

            NavControllerOutput obj = new NavControllerOutput(message);

            Assert.AreEqual(data.alt_error, obj.alt_error);
            Assert.AreEqual(data.aspd_error, obj.aspd_error);
            Assert.AreEqual(data.nav_bearing, obj.nav_bearing);
            Assert.AreEqual(data.nav_pitch, obj.nav_pitch);
            Assert.AreEqual(data.nav_roll, obj.nav_roll);
            Assert.AreEqual(data.target_bearing, obj.target_bearing);
            Assert.AreEqual(data.wp_dist, obj.wp_dist);
            Assert.AreEqual(data.xtrack_error, obj.xtrack_error);

            NavControllerOutputDTO dto = new NavControllerOutputDTO(obj);

            Assert.AreEqual(dto.alt_error, obj.alt_error);
            Assert.AreEqual(dto.aspd_error, obj.aspd_error);
            Assert.AreEqual(dto.nav_bearing, obj.nav_bearing);
            Assert.AreEqual(dto.nav_pitch, obj.nav_pitch);
            Assert.AreEqual(dto.nav_roll, obj.nav_roll);
            Assert.AreEqual(dto.target_bearing, obj.target_bearing);
            Assert.AreEqual(dto.wp_dist, obj.wp_dist);
            Assert.AreEqual(dto.xtrack_error, obj.xtrack_error);

        }

        [TestMethod]
        public void CheckVfrHudObject()
        {
            MAVLink.mavlink_vfr_hud_t data = new MAVLink.mavlink_vfr_hud_t();
            data.airspeed = 1;
            data.alt = 2;
            data.climb = 3;
            data.groundspeed = 4;
            data.heading = 5;
            data.throttle = 6;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.VFR_HUD, data);

            VfrHud obj = new VfrHud(message);

            Assert.AreEqual(data.airspeed, obj.airspeed);
            Assert.AreEqual(data.alt, obj.alt);
            Assert.AreEqual(data.climb, obj.climb);
            Assert.AreEqual(data.groundspeed, obj.groundspeed);
            Assert.AreEqual(data.heading, obj.heading);
            Assert.AreEqual(data.throttle, obj.throttle);

            VfrHudDTO dto = new VfrHudDTO(obj);

            Assert.AreEqual(dto.airspeed, obj.airspeed);
            Assert.AreEqual(dto.alt, obj.alt);
            Assert.AreEqual(dto.climb, obj.climb);
            Assert.AreEqual(dto.groundspeed, obj.groundspeed);
            Assert.AreEqual(dto.heading, obj.heading);
            Assert.AreEqual(dto.throttle, obj.throttle);

        }

        [TestMethod]
        public void CheckTerrainReportObject()
        {
            MAVLink.mavlink_terrain_report_t data = new MAVLink.mavlink_terrain_report_t();
            data.current_height = 1;
            data.lat = 2;
            data.loaded = 3;
            data.lon = 4;
            data.pending = 5;
            data.spacing = 6;
            data.terrain_height = 7;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.TERRAIN_REPORT, data);

            TerrainReport obj = new TerrainReport(message);

            Assert.AreEqual(data.current_height, obj.current_height);
            Assert.AreEqual(data.lat, obj.lat);
            Assert.AreEqual(data.loaded, obj.loaded);
            Assert.AreEqual(data.lon, obj.lon);
            Assert.AreEqual(data.pending, obj.pending);
            Assert.AreEqual(data.spacing, obj.spacing);
            Assert.AreEqual(data.terrain_height, obj.terrain_height);

            TerrainReportDTO dto = new TerrainReportDTO(obj);

            Assert.AreEqual(dto.current_height, obj.current_height);
            Assert.AreEqual(dto.lat, obj.lat);
            Assert.AreEqual(dto.loaded, obj.loaded);
            Assert.AreEqual(dto.lon, obj.lon);
            Assert.AreEqual(dto.pending, obj.pending);
            Assert.AreEqual(dto.spacing, obj.spacing);
            Assert.AreEqual(dto.terrain_height, obj.terrain_height);

        }

        [TestMethod]
        public void CheckScaledImu2DTO()
        {
            MAVLink.mavlink_scaled_imu2_t data = new MAVLink.mavlink_scaled_imu2_t();
            data.time_boot_ms = 1;
            data.xacc = 2;
            data.xgyro = 3;
            data.xmag = 4;
            data.yacc = 5;
            data.ygyro = 6;
            data.ymag = 7;
            data.zacc = 8;
            data.zgyro = 9;
            data.zmag = 10;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.SCALED_IMU2, data);

            ScaledImu2 obj = new ScaledImu2(message);

            Assert.AreEqual(data.time_boot_ms, obj.time_boot_ms);
            Assert.AreEqual(data.xacc, obj.xacc);
            Assert.AreEqual(data.xgyro, obj.xgyro);
            Assert.AreEqual(data.xmag, obj.xmag);
            Assert.AreEqual(data.yacc, obj.yacc);
            Assert.AreEqual(data.ygyro, obj.ygyro);
            Assert.AreEqual(data.ymag, obj.ymag);
            Assert.AreEqual(data.zacc, obj.zacc);
            Assert.AreEqual(data.zgyro, obj.zgyro);
            Assert.AreEqual(data.zmag, obj.zmag);

            ScaledImu2DTO dto = new ScaledImu2DTO(obj);

            Assert.AreEqual(dto.time_boot_ms, obj.time_boot_ms);
            Assert.AreEqual(dto.xacc, obj.xacc);
            Assert.AreEqual(dto.xgyro, obj.xgyro);
            Assert.AreEqual(dto.xmag, obj.xmag);
            Assert.AreEqual(dto.yacc, obj.yacc);
            Assert.AreEqual(dto.ygyro, obj.ygyro);
            Assert.AreEqual(dto.ymag, obj.ymag);
            Assert.AreEqual(dto.zacc, obj.zacc);
            Assert.AreEqual(dto.zgyro, obj.zgyro);
            Assert.AreEqual(dto.zmag, obj.zmag);

        }

        [TestMethod]
        public void CheckPowerStatusObject()
        {
            MAVLink.mavlink_power_status_t data = new MAVLink.mavlink_power_status_t();
            data.flags = 1;
            data.Vcc = 2;
            data.Vservo = 3;

            MavLinkMessage message = createSampleMessage(MAVLink.MAVLINK_MSG_ID.POWER_STATUS, data);

            PowerStatus obj = new PowerStatus(message);

            Assert.AreEqual(data.Vservo, obj.Vservo);
            Assert.AreEqual(data.Vcc, obj.Vcc);
            Assert.AreEqual(1, obj.flags.Count);

            PowerStatusDTO dto = new PowerStatusDTO(obj);

            Assert.AreEqual(dto.Vservo, obj.Vservo);
            Assert.AreEqual(dto.Vcc, obj.Vcc);
            Assert.AreEqual(1, dto.flags.Count);

        }
    }
}
