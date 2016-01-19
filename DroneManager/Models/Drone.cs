using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Models;
using DroneConnection;

namespace DroneManager.Models
{
    public class Drone
    {
        public DroneEntity data = new DroneEntity();

        public Drone(DroneEntity entity)
        {
            data.copy(entity);
        }

        public DroneLink connection { get; set; }
    }
}
