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
        // database record
        public DroneEntity data = new DroneEntity();

        // live connection
        public DroneLink connection { get; set; }

        public Drone(DroneEntity entity)
        {
            data.copy(entity);
        }

    }
}
