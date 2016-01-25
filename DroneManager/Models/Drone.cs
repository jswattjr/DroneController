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

        public Boolean isConnected()
        {
            if (null != connection)
            {
                if (connection.port.IsOpen)
                {
                    return true;
                }
            }
            return false;
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

        /*
        public List<Object> getRawMessageFeed()
        {
            int readSize = 100;
            List<Object> messages = new List<Object>();
            if (this.isConnected())
            {
                FixedSizedQueue<MavLinkMessage> queue = this.connection.readQueue;
                while((queue.Count > 0)&&(readSize > 0))
                {
                    MavLinkMessage message;
                    queue.TryDequeue(message)
                    messages.Add()
                }
                return 
            }
            else
            {
                logger.Error("getRawMessageFeed() called on disconnected drone, returning null.");
                return null;
            }
        }
        */

    }
}
