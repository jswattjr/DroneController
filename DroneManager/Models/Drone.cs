using System;
using DataAccessLibrary.Models;
using DroneConnection;
using NLog;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;

namespace DroneManager.Models
{
    public class Drone
    {

        static Logger logger = LogManager.GetCurrentClassLogger();

        // database record
        public DroneEntity data = new DroneEntity();

        // live connection
        public MavLinkConnection connection { get; set; }

        // events connection with MavLinkConnection
        MavLinkEvents events;
        // RabbitMQ consumer identifier ID
        string consumerTag;

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

        // attempts to open listen feed
        public Boolean openMessageFeed()
        {
            logger.Debug("Opening Listening/Processing Feed for {0}", connection.port.PortName);
           
            events = new MavLinkEvents(connection.systemId, connection.componentId);
            if ((null == events) || (null == events.channel))
            {
                logger.Error("Failed to open event message queue for {0}", connection.port.PortName);
                return false;
            }

            logger.Debug("Creating Basic Consumer");
            EventingBasicConsumer consumer = new EventingBasicConsumer(events.channel);

            logger.Debug("Adding callback function");
            consumer.Received += (model, ea) =>
            {
                eventsCallback(ea);
            };

            logger.Debug("Inserting consumer into the channel");
            this.consumerTag = events.channel.BasicConsume(queue: events.getMessageQueueName(),
                                    noAck: true,
                                    consumer: consumer);

            logger.Debug("Consumer created successfully");
            return true;
        }

        void eventsCallback(BasicDeliverEventArgs eventArguments)
        {
            try
            {
                var body = eventArguments.Body;
                String jsonBody = Encoding.UTF8.GetString(body);
                MavLinkMessage message = JsonConvert.DeserializeObject<MavLinkMessage>(jsonBody);
                if (message.messid.Equals(MAVLink.MAVLINK_MSG_ID.HEARTBEAT))
                {
                    logger.Debug("Heartbeat received on port {0}", connection.port.PortName);
                }
            }
            catch (Exception e)
            {
                logger.Error("Failure in parsing message, exception with message {0}", e.Message);
            }
        }
    }
}
