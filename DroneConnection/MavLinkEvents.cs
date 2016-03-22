using Newtonsoft.Json;
using NLog;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneConnection
{
    // Messaging object for MavLinkMessages
    // Takes systemId and ComponentId to create unique queue name

    public class MavLinkEvents
    {
        static Logger logger = LogManager.GetLogger("database");
        
        // string prefixed to message queue names in front of system id number
        const String messageQueuePrefix = "MavLinkConnection";
        
        // RabbitMQ capabilities, CreateMessageQueue to initialize, DisposeMessageQueue to delete
        public ConnectionFactory eventFactory { get; private set; } = new ConnectionFactory();
        public IConnection connection { get; private set; }
        public IModel channel { get; private set; }

        QueueDeclareOk messageQueueExists = null;


        // unique IDs for connection
        int systemId;
        int componentId;

        // initialize queue
        public MavLinkEvents(int systemId, int componentId)
        {
            this.systemId = systemId;
            this.componentId = componentId;
            this.initializeMessageQueue();
        }

        // cleanup queue
        ~MavLinkEvents()
        {
            this.destroyQueue();
        }

        public String getMessageQueueName()
        {
            return messageQueuePrefix + "_" + systemId.ToString();
        }

        // initializes queue variables, needs to be paired with 'destroyqueue' to release resources
        void initializeMessageQueue()
        {
            try
            {
                this.connection = eventFactory.CreateConnection();
                this.channel = connection.CreateModel();
                logger.Debug("RabbitMQ channel and factory created successfully.");
            }
            catch (Exception e)
            {
                logger.Debug("Exception while initializing message queue. Please check that target RabbitMQ server exists.");
                logger.Debug("Exception message: {0}", e.Message);
                if (null != connection)
                {
                    connection.Dispose();
                }
                if (null != channel)
                {
                    channel.Dispose();
                }
                connection = null;
                channel = null;
            }
        }


        // declares the message queue, needs to be called after sysid is fetched from target system, so id can be used in queue name
        void declareQueue()
        {
            if ((null == channel) || (null == connection))
            {
                return;
            }
            this.messageQueueExists = channel.QueueDeclare(
                queue: getMessageQueueName(),
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            logger.Debug("RabbitMQ Message Queue Created for {0}", this.getMessageQueueName());
        }

        // releases queue resources
        public void destroyQueue()
        {
            if ((null == channel) || (null == connection))
            {
                return;
            }
            connection.Dispose();
            channel.Dispose();
            this.channel = null;
            this.connection = null;
        }

        // function for posting message to queue
        public void postToMessageQueue(MavLinkMessage message)
        {
            if ((null == channel) || (null == connection))
            {
                return;
            }
            if (null == this.messageQueueExists)
            {
                this.declareQueue();
            }

            // rabbitmq only supports string, so convert to JSON then drop it in
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            String jsonObject = JsonConvert.SerializeObject(message, settings);

            // message properties
            IBasicProperties props = channel.CreateBasicProperties();

            // encode message
            var messageBody = Encoding.UTF8.GetBytes(jsonObject);

            // publish message
            this.channel.BasicPublish("", this.getMessageQueueName(), props, messageBody);
            logger.Trace("RabbitMQ Message {1} published for {0}", this.getMessageQueueName(), jsonObject);

            if (message.messid.Equals(MAVLink.MAVLINK_MSG_ID.HEARTBEAT))
            {
                logger.Trace("RabbitMQ Message {1} published for {0}", this.getMessageQueueName(), jsonObject);
            }
        }


    }
}
