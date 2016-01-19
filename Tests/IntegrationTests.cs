using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneManager.Models;
using DataAccessLibrary.Models;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary;

namespace Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void DroneEntityPersistanceTest()
        {
            // dummy create
            IEntityRepository<DroneEntity> droneRepo = RepositoryFactory.getDroneRepository();

            String testString = "foobar";

            // create dummy drone record
            DroneEntity connectionRecord = new DroneEntity();
            connectionRecord.serialPort = testString;
            connectionRecord.name = testString;
            connectionRecord.copy(droneRepo.create(connectionRecord));

            // dummy search
            DroneEntity lookupDrone = droneRepo.getById(connectionRecord.id);

            Assert.IsTrue(lookupDrone.id.Equals(connectionRecord.id));
            Assert.IsTrue(lookupDrone.name.Equals(testString));
            Assert.IsTrue(lookupDrone.serialPort.Equals(testString));

            // delete
            droneRepo.delete(lookupDrone);

            lookupDrone = droneRepo.getById(connectionRecord.id);

            Assert.IsNull(lookupDrone);
        }
        
    }
}
