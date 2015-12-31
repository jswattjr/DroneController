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
    public class Drone : DroneEntity
    {
        public Drone(DroneEntity entity)
        {
            this.copy(entity);
        }

        public Drone()
        {

        }

        public DroneLink connection { get; set; }
    }
}
