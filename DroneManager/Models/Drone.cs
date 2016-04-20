using System;
using DroneConnection;
using NLog;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;
using System.Collections.Generic;
using DroneManager.Models.MessageContainers;
using System.Threading;

namespace DroneManager.Models
{
    public partial class Drone
    {

        static Logger logger = LogManager.GetLogger("applog");
        
        // special logger defined in nlog config for drone messages
        static Logger messageDump = LogManager.GetLogger("rawmessages");

        /// <summary>
        /// Accessor to serial port connection
        /// </summary>
        public MavLinkConnection connection { get; private set; }

        /// <summary>
        /// Connection Unique Identifier
        /// </summary>
        public Guid id { get; set; }

        // events connection with MavLinkConnection
        MavLinkEvents events;

        // multithreaded object where requests await responses from the device after command send messages
        Dictionary<MAVLink.MAV_CMD, Stack<CommandAck>> commandAckStacks = new Dictionary<MAVLink.MAV_CMD, Stack<CommandAck>>();

        // multithreaded object where requests await responses from the device after parameter set messages
        ParamValue parameterSetAckObj = null;

        // lock object for parameter set requests
        Object parameterSetLock = new object();

        // timeout value for parameter set requets
        uint parameterSetTimeout_ms = 2000;
        int parameterSetPollrate_ms = 100;

        // RabbitMQ consumer identifier ID
        string consumerTag;

        // Dictionary of all current device parameter values
        Dictionary<MAVLink.MAVLINK_MSG_ID, MavLinkMessage> currentState = new Dictionary<MAVLink.MAVLINK_MSG_ID, MavLinkMessage>();

        /// <summary>
        /// Constructor, takes a live MavLinkConnection object and begins monitoring device state
        /// </summary>
        /// <param name="connection">live, connected MavLinkConnection. Call MavLinkConnection.createConnection(port) and check port is open prior to calling this.</param>
        public Drone(MavLinkConnection connection)
        {
            this.id = Guid.NewGuid();
            this.connection = connection;
            this.openMessageFeed();
        }


        /// <summary>
        /// Checks connection status of object by querying serial port state
        /// </summary>
        /// <returns>
        /// Returns true if port is open.
        /// </returns>
        public Boolean isConnected()
        {
            if (null != connection)
            {
                if (connection.isOpen())
                {
                    return true;
                }
                logger.Debug("Drone {0} is not connected on port {1}. Port is closed.", this.id, connection.portName());
            }
            return false;
        }

        /// <summary>
        /// Dictionary of all current connected device parameter values
        /// </summary>
        public Dictionary<String, ParamValue> Parameters { get {
                if (null != parameters)
                {
                    lock (parameters)
                    {
                        return new Dictionary<String, ParamValue>(parameters);
                    }
                }
                return null;
            } private set { parameters = value; } }

        private Dictionary<String, ParamValue> parameters = null;

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

        public Attitude getAttitude()
        {
            return getCurrentMessage<Attitude>(MAVLink.MAVLINK_MSG_ID.ATTITUDE);
        }

        public GlobalPositionInt getGlobalPositionInt()
        {
            return getCurrentMessage<GlobalPositionInt>(MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT);
        }

        public RcChannelsRaw getRcChannelsRaw()
        {
            return getCurrentMessage<RcChannelsRaw>(MAVLink.MAVLINK_MSG_ID.RC_CHANNELS_RAW);
        }

        public ServoOutputRaw getServerOutputRaw()
        {
            return getCurrentMessage<ServoOutputRaw>(MAVLink.MAVLINK_MSG_ID.SERVO_OUTPUT_RAW);
        }

        public MissionCurrent getMissionCurrent()
        {
            return getCurrentMessage<MissionCurrent>(MAVLink.MAVLINK_MSG_ID.MISSION_CURRENT);
        }

        public NavControllerOutput getNavControllerOutput()
        {
            return getCurrentMessage<NavControllerOutput>(MAVLink.MAVLINK_MSG_ID.NAV_CONTROLLER_OUTPUT);
        }

        public VfrHud getVfrHud()
        {
            return getCurrentMessage<VfrHud>(MAVLink.MAVLINK_MSG_ID.VFR_HUD);
        }

        public TerrainReport getTerrainReport()
        {
            return getCurrentMessage<TerrainReport>(MAVLink.MAVLINK_MSG_ID.TERRAIN_REPORT);
        }

        public ScaledImu2 getScaledImu2()
        {
            return getCurrentMessage<ScaledImu2>(MAVLink.MAVLINK_MSG_ID.SCALED_IMU2);
        }

        public PowerStatus getPowerStatus()
        {
            return getCurrentMessage<PowerStatus>(MAVLink.MAVLINK_MSG_ID.POWER_STATUS);
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

        // start listening to device message feed
        private Boolean openMessageFeed()
        {
            logger.Debug("Opening Listening/Processing Feed for {0}", connection.portName());
           
            events = new MavLinkEvents(connection.systemId, connection.componentId);
            if ((null == events) || (null == events.channel))
            {
                logger.Error("Failed to open event message queue for {0}", connection.portName());
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

        /// <summary>
        /// Set Parameter on target device.
        /// </summary>
        /// <param name="parameterName">String unique identifier for parameter</param>
        /// <param name="parameterValue">Parameter value in string format</param>
        /// <param name="type">Parameter type, enumerated in MAV_PARAM_TYPE</param>
        public Boolean setParameter(string parameterName, Single parameterValue, MAVLink.MAV_PARAM_TYPE type)
        {
            if (this.isConnected())
            {
                // don't send updates for same values
                if (this.Parameters.ContainsKey(parameterName))
                {
                    if (this.parameters[parameterName].param_value.Equals(parameterValue))
                    {
                        return true;
                    }
                }

                // wait here on multiple parameter set requests
                lock (parameterSetLock)
                {
                    connection.sendParamUpdate(parameterName, parameterValue, type);
                    return fetchParameterSetAck(parameterName);
                }
            }
            return false;
        }


        private Boolean fetchParameterSetAck(String parameterName)
        {
            DateTime deadline = DateTime.Now.AddMilliseconds(this.parameterSetTimeout_ms);
            while (DateTime.Now < deadline)
            {
                if ((null != this.parameterSetAckObj)&&(this.parameterSetAckObj.param_id.Equals(parameterName)))
                {
                    this.parameterSetAckObj = null;
                    return true;
                }
                // sleep 
                Thread.Sleep(this.parameterSetPollrate_ms);
            }
            logger.Error("Error: timeout waiting for result for setting parameter {0}", parameterName);
            return false;
        }

        // function for processing new parameter message
        private void parameterReceived(ParamValue param)
        {
            if (this.parameters.ContainsKey(param.param_id))
            {
                this.parameters[param.param_id] = param;
            }
            else
            {
                this.parameters.Add(param.param_id, param);
            }
            logger.Debug("Parameter {0} received with value {1}", param.param_id, param.param_value);
        }

        // main events function, which processes messages from the device
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

                // log message text to file
                messageDump.Trace(jsonBody);

                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                MavLinkMessage message = JsonConvert.DeserializeObject<MavLinkMessage>(jsonBody, settings);
                if (null == message)
                {
                    logger.Error("Failed to parse MavLinkMessage from JSON in events callback");
                    return;
                }

                // store message in currentState Dictionary
                this.currentState[(MAVLink.MAVLINK_MSG_ID)message.messid] = message;

                // commands wait for their ack events and fetch them from these queues
                if (message.messid.Equals(MAVLink.MAVLINK_MSG_ID.COMMAND_ACK))
                {
                    CommandAck cmdack = new CommandAck(message);
                    if (!this.commandAckStacks.ContainsKey(cmdack.command))
                    {
                        this.commandAckStacks[cmdack.command] = new Stack<CommandAck>();
                    }
                    this.commandAckStacks[cmdack.command].Push(cmdack);
                }

                if (message.messid.Equals(MAVLink.MAVLINK_MSG_ID.HEARTBEAT))
                {
                    logger.Trace("Heartbeat received on port {0} {1}", connection.portName(), jsonBody);
                }

                if (null == parameters)
                {
                    parameters = new Dictionary<String, ParamValue>();
                    this.connection.sendParamsListRequest();
                }

                if ((message.messid.Equals(MAVLink.MAVLINK_MSG_ID.PARAM_VALUE))&&(null != parameters))
                {
                    lock (parameters)
                    {
                        // set this value in the global parameter set
                        ParamValue param = new ParamValue(message);
                        parameterReceived(param);

                        // set this object in case there is a thread waiting on a param_value 'ack' message on a param set request
                        parameterSetAckObj = param;
                    }
                }

            }
            catch (Exception e)
            {
                logger.Error("Failure in parsing message, exception with message {0}", e.Message);
            }
        }
    }
}
