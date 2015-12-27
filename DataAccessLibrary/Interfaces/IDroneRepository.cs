using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Models;

namespace DataAccessLibrary.Interfaces
{
    public interface IDroneRepository
    {
            
        IEnumerable<DroneEntity> getAll();

        DroneEntity getById(Guid id);

        DroneEntity set(DroneEntity entity);

        DroneEntity create(DroneEntity entity);

        void delete(DroneEntity entity);
    }
}
