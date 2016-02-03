using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneConnection;
using Newtonsoft.Json;
using DroneManager.Models.MessageContainers;

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
        public void CheckCopySimilarStructToProperties()
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

        }

        [TestMethod]
        public void CopySimilarStressTest()
        {
            // 0.644s with direct copy
            // 8s with reflection
            int testsize = 1000000;
            for (int count = 0; count < testsize; count++)
            {
                CheckCopySimilarStructToProperties();
            }
        }
    }
}
