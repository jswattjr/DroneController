using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneConnection;
using Newtonsoft.Json;

namespace Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void CheckMavLinkMessageSerialization()
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
    }
}
