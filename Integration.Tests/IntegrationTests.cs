using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneManager.Models;
using DataAccessLibrary.Models;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary;
using DataAccessLibrary.Repositories;
using System.Collections.Generic;
using System.Linq;
using NLog;

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

        [TestMethod]
        public void LoggingTest()
        {
            LogRepository logRepo = DataAccessLibrary.RepositoryFactory.getLogsRepository();
            IList<NLogEntity> logs = logRepo.getAll().ToList();
            int size = logs.Count;
            Logger logger = LogManager.GetLogger("database");
            logger.Debug("LoggingTest run, sample log entry");
            logs = logRepo.getAll().ToList();
            Assert.AreEqual(logs.Count, size + 1);
        }
        
        [TestMethod]
        public void SettingsTest()
        {
            String settingName = "test";
            String settingValue = "value";
            IEntityRepository<SettingEntity> repo = RepositoryFactory.getSettingRepository();

            SettingEntity settingEntity = repo.getByName(settingName);
            if (null != settingEntity)
            {
                repo.delete(settingEntity);
                settingEntity = repo.getByName(settingName);
            }
            Assert.IsNull(settingEntity);

            SettingEntity testSetting = new SettingEntity();
            testSetting.name = settingName;
            testSetting.value = settingValue;
            repo.create(testSetting);

            settingEntity = repo.getByName(settingName);
            Assert.IsNotNull(settingEntity);

            Assert.AreEqual(settingEntity.value, testSetting.value);

        }
    }
}
