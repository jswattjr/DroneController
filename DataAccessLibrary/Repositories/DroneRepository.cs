using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Models;

namespace DataAccessLibrary.Repositories
{
    class DroneRepository : IDroneRepository
    {
        AdoDatabaseAccess dbAccess = new AdoDatabaseAccess();

        public DroneEntity create(DroneEntity entity)
        {
            entity.id = Guid.NewGuid();
            dbAccess.DroneEntities.Add(entity);
            return entity;
        }

        public void delete(DroneEntity entity)
        {
            DroneEntity target = getById(entity.id);
            if (null != target)
            {
                dbAccess.DroneEntities.Remove(target);
            }
        }

        public IEnumerable<DroneEntity> getAll()
        {
            return dbAccess.DroneEntities;
        }

        public DroneEntity getById(Guid id)
        {
            DroneEntity target = dbAccess.DroneEntities.Find(id);
            return target;
        }

        public DroneEntity set(DroneEntity entity)
        {
            DroneEntity target = getById(entity.id);
            target.Set(entity);
            return target;
        }
    }
}
