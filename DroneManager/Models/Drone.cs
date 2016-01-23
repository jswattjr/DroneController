using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Models;
using DroneConnection;
using NLog;

namespace DroneManager.Models
{
    public class Drone
    {

        static Logger logger = LogManager.GetCurrentClassLogger();

        // database record
        public DroneEntity data = new DroneEntity();

        // live connection
        public MavLinkConnection connection { get; set; }

        public Drone(DroneEntity entity)
        {
            data.copy(entity);
        }

        public void arm()
        {
            connection.sendArmMessage();
        }

        public void disarm()
        {
            connection.sendArmMessage(false);
        }

        public void returnToLand()
        {

        }

        public void land()
        {

        }

    }
}
