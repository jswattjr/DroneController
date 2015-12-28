using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Repositories;
using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public class RepositoryFactory
    {
        public static IEntityRepository<DroneEntity> getDroneRepository()
        {
            AdoDatabaseAccess dbContext = new AdoDatabaseAccess();
            GenericEntityRepository<DroneEntity> repo = new GenericEntityRepository<DroneEntity>(dbContext.DroneEntities, dbContext);
            return repo;
        }

        public static IEntityRepository<SettingEntity> getSettingRepository()
        {
            AdoDatabaseAccess dbContext = new AdoDatabaseAccess();
            GenericEntityRepository<SettingEntity> repo = new GenericEntityRepository<SettingEntity>(dbContext.SettingEntities, dbContext);
            return repo;
        }
    }
}
