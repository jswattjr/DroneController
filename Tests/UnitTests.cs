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
    }
}
