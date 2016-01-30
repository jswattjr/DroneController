using System;
using DataAccessLibrary.Models;
using DroneConnection;
using NLog;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;

namespace DroneManager.Models
{
    public class Drone
    {

        static Logger logger = LogManager.GetCurrentClassLogger();

        // database record
        public DroneEntity data = new DroneEntity();

        // live connection
        public MavLinkConnection connection
        {
            get
            {
                return connection;
            }
            set
            {
                //openMessageFeed();
            }
        }

        // events connection with MavLinkConnection
        MavLinkEvents events;

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
            var consumer = new EventingBasicConsumer(events.channel);

            logger.Debug("Adding callback function");
            consumer.Received += (model, ea) =>
            {
                Drone.eventsCallback(ea);
            };

            logger.Debug("Inserting consumer into the channel");
            events.channel.BasicConsume(queue: events.getMessageQueueName(),
                                    noAck: true,
                                    consumer: consumer);

            logger.Debug("Consumer created successfully");
            return true;
        }

        static void eventsCallback(BasicDeliverEventArgs eventArguments)
        {
            var body = eventArguments.Body;
            String jsonBody = Encoding.UTF8.GetString(body);
            MavLinkMessage message = JsonConvert.DeserializeObject<MavLinkMessage>(jsonBody);
            logger.Debug("Message received by listener: {0}", jsonBody);
        }
    }
}
