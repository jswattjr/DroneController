using DroneConnection;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DroneManager.Models.MessageContainers
{
    public abstract class MessageContainerBase
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public abstract MAVLink.MAVLINK_MSG_ID MessageID { get; }
        public abstract Type getStructType();
 
        public MavLinkMessage message;       

        public MessageContainerBase(MavLinkMessage message)
        {
            this.message = message;
            if (message.messid.Equals(MessageID))
            {
                try
                {
                    MethodInfo method = typeof(MessageContainerBase).GetMethod("copyMessageValues", (BindingFlags.NonPublic | BindingFlags.Instance));
                    MethodInfo generic = method.MakeGenericMethod(getStructType());
                    generic.Invoke(this, null);
                }
                catch (Exception e)
                {
                    logger.Error("Unable to parse data for {1} object, with exception message {0}", e.Message, MessageID);
                }
            }
            else
            {
                logger.Error("Tried to initialize {1} object with message of type {0}", (MAVLink.MAVLINK_MSG_ID)message.messid, MessageID);
            }

        }

        // Method name is referenced via reflection, do not change!
        protected void copyMessageValues<T>() where T : struct
        {
            T raw_data = (T)message.data_struct;

            // copies all like-named properties from struct to this object
            CopySimilar.StructToProperties<T>(raw_data, this);
        }

    }
}
