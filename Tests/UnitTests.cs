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

            MavLinkMessage sampleMessage = new MavLinkMessage();
            sampleMessage.messid = MAVLink.MAVLINK_MSG_ID.HEARTBEAT;
            sampleMessage.seq = 128;
            sampleMessage.sysid = 12;
            sampleMessage.compid = 12;
            sampleMessage.data_struct = sampleStruct;

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


        }
    }
}
