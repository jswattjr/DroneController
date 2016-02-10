using System;
using DataAccessLibrary.Models;
using DroneConnection;
using NLog;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;
using System.Collections.Generic;
using DroneManager.Models.MessageContainers;

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

        Dictionary<MAVLink.MAVLINK_MSG_ID, MavLinkMessage> currentState = new Dictionary<MAVLink.MAVLINK_MSG_ID, MavLinkMessage>();

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


        // commands
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

        //state
        public Heartbeat getHearbeat()
        {
            return getCurrentMessage<Heartbeat>(MAVLink.MAVLINK_MSG_ID.HEARTBEAT);
        }

        public SystemStatus getSystemStatus()
        {
            return getCurrentMessage<SystemStatus>(MAVLink.MAVLINK_MSG_ID.SYS_STATUS);
        }

        public SystemTime getSystemTime()
        {
            return getCurrentMessage<SystemTime>(MAVLink.MAVLINK_MSG_ID.SYSTEM_TIME);
        }

        public GpsRawInt getGpsRawInt()
        {
            return getCurrentMessage<GpsRawInt>(MAVLink.MAVLINK_MSG_ID.GPS_RAW_INT);
        }

        public RawImu getRawImu()
        {
            return getCurrentMessage<RawImu>(MAVLink.MAVLINK_MSG_ID.RAW_IMU);
        }

        public ScaledPressure getScaledPressure()
        {
            return getCurrentMessage<ScaledPressure>(MAVLink.MAVLINK_MSG_ID.SCALED_PRESSURE);
        }

        T getCurrentMessage<T>(MAVLink.MAVLINK_MSG_ID id) where T : MessageContainerBase
        {
            if (!this.currentState.ContainsKey(id))
            {
                return null;
            }
            MavLinkMessage message = this.currentState[id];
            return (T)Activator.CreateInstance(typeof(T), new object[] { message });
        }

        // events
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
                if (null == eventArguments)
                {
                    logger.Error("events callback with no eventArguments");
                    return;
                }
                var body = eventArguments.Body;
                if (null == body)
                {
                    logger.Error("events callback with null body");
                    return;
                }
                String jsonBody = Encoding.UTF8.GetString(body);
                if (null == jsonBody)
                {
                    logger.Error("failed to parse JSON in events callback");
                    return;
                }

                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                MavLinkMessage message = JsonConvert.DeserializeObject<MavLinkMessage>(jsonBody, settings);
                if (null == message)
                {
                    logger.Error("Failed to parse MavLinkMessage from JSON in events callback");
                    return;
                }

                // store message in currentState Dictionary
                this.currentState[(MAVLink.MAVLINK_MSG_ID)message.messid] = message;

                if (message.messid.Equals(MAVLink.MAVLINK_MSG_ID.HEARTBEAT))
                {
                    logger.Error("Heartbeat received on port {0} {1}", connection.port.PortName, jsonBody);
                }
            }
            catch (Exception e)
            {
                logger.Error("Failure in parsing message, exception with message {0}", e.Message);
            }
        }
    }
}
