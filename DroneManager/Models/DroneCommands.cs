using DroneConnection;
using DroneManager.Models.MessageContainers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DroneManager.Models
{
    public partial class Drone
    {
        private Commands command = null;
        private Object lockObj = new object();
        public Commands Command
        {
            get
            {
                // double check lock, likely overkill for this but implemented for fun.
                if (null == command)
                {
                    lock (lockObj)
                    {
                        if (null == command)
                        {
                            command = new Commands(this);
                        }
                    }
                }
                return command;
            }
            private set
            {
                command = value;
            }
        }

        private List<Task<CommandAck>> taskCommands = new List<Task<CommandAck>>();

        /*
        https://pixhawk.ethz.ch/mavlink/ , MAV_CMD
        MAV_CMD
        Commands to be executed by the MAV. 
        They can be executed on user request, or as part of a mission script. 
        If the action is used in a mission, 
        the parameter mapping to the waypoint/mission message is as follows: 
        Param 1, Param 2, Param 3, Param 4, X: Param 5, Y:Param 6, Z:Param 7. 
        This command list is similar what ARINC 424 is for commercial aircraft: 
        A data format how to interpret waypoint/mission data.
        */

        public class Commands
        {
            static Logger logger = LogManager.GetLogger("applog");

            Drone drone;
            MavLinkConnection connection;

            static int cmdpoll_ms = 5;
            static int cmdtimeout_ms = 3000;

            public Commands(Drone droneObj)
            {
                connection = droneObj.connection;
                drone = droneObj;
            }


            private CommandAck fetchCommandAck(MAVLink.MAV_CMD cmdType)
            {
                DateTime deadline = DateTime.Now.AddMilliseconds(cmdtimeout_ms);
                while (DateTime.Now < deadline)
                {
                    if (drone.commandAckStacks.ContainsKey(cmdType))
                    {
                        Stack <CommandAck> stack = drone.commandAckStacks[cmdType];
                        if (stack.Count > 0)
                        {
                            CommandAck result = stack.Pop();
                            logger.Debug("Command Ack received for {0} command with status {1}", cmdType, result.result.ToString());
                            return result;
                        }
                    }
                    // sleep 
                    Thread.Sleep(cmdpoll_ms);
                }
                logger.Error("Error: timeout waiting for command result for {0}", cmdType);
                return null;
            }

            /*
            400	MAV_CMD_COMPONENT_ARM_DISARM	Arms / Disarms a component
            Mission Param #1	1 to arm, 0 to disarm
            */
            public CommandAck arm()
            {
                return runCommand(MAVLink.MAV_CMD.COMPONENT_ARM_DISARM, Convert.ToInt32(true), default(Int32),
                    default(Int32), default(Int32), default(Int32), default(Int32),
                    default(Int32));
            }

            public CommandAck disarm()
            {
                return runCommand(MAVLink.MAV_CMD.COMPONENT_ARM_DISARM, Convert.ToInt32(false), default(Int32),
                    default(Int32), default(Int32), default(Int32), default(Int32),
                    default(Int32));
            }

            /*
            22	MAV_CMD_NAV_TAKEOFF	Takeoff from ground / hand
            Mission Param #1	Minimum pitch (if airspeed sensor present), desired pitch without sensor
            Mission Param #2	Empty
            Mission Param #3	Empty
            Mission Param #4	Yaw angle (if magnetometer present), ignored without magnetometer
            Mission Param #5	Latitude
            Mission Param #6	Longitude
            Mission Param #7	Altitude
            */
            public CommandAck takeoff(Int32 heightInMeters)
            {
                return runCommand(MAVLink.MAV_CMD.TAKEOFF, default(Int32), default(Int32),
                    default(Int32), default(Int32), default(Int32), default(Int32),
                    heightInMeters);
            }

            /*
            21	MAV_CMD_NAV_LAND	Land at location
            Mission Param #1	Abort Alt
            Mission Param #2	Empty
            Mission Param #3	Empty
            Mission Param #4	Desired yaw angle
            Mission Param #5	Latitude
            Mission Param #6	Longitude
            Mission Param #7	Altitude
            */
            public CommandAck landAtLocation(Int32 latitude, Int32 longitude, Int32 altitude = 1)
            {
                return runCommand(MAVLink.MAV_CMD.LAND, default(Int32), default(Int32),
                    default(Int32), default(Int32), latitude, longitude,
                    altitude);
            }

  
            /*
            16	MAV_CMD_NAV_WAYPOINT	Navigate to MISSION.
            Mission Param #1	Hold time in decimal seconds. (ignored by fixed wing, time to stay at MISSION for rotary wing)
            Mission Param #2	Acceptance radius in meters (if the sphere with this radius is hit, the MISSION counts as reached)
            Mission Param #3	0 to pass through the WP, if > 0 radius in meters to pass by WP. Positive value for clockwise orbit, negative value for counter-clockwise orbit. Allows trajectory control.
            Mission Param #4	Desired yaw angle at MISSION (rotary wing)
            Mission Param #5	Latitude
            Mission Param #6	Longitude
            Mission Param #7	Altitude
            */

            public CommandAck navigateWaypoint(Int32 latitude, Int32 longitude, Int32 altitude)
            {
                return runCommand(MAVLink.MAV_CMD.WAYPOINT, default(Int32), default(Int32),
                    default(Int32), default(Int32), latitude, longitude,
                    altitude);
            }

            /*
            17	MAV_CMD_NAV_LOITER_UNLIM	Loiter around this MISSION an unlimited amount of time
            Mission Param #1	Empty
            Mission Param #2	Empty
            Mission Param #3	Radius around MISSION, in meters. If positive loiter clockwise, else counter-clockwise
            Mission Param #4	Desired yaw angle.
            Mission Param #5	Latitude
            Mission Param #6	Longitude
            Mission Param #7	Altitude
            */

            public CommandAck loiterUnlimited(Int32 radiusMeters, Int32 latitude, Int32 longitude, Int32 altitude)
            {
                return runCommand(MAVLink.MAV_CMD.LOITER_UNLIM, default(Int32), default(Int32),
                    radiusMeters, default(Int32), latitude, longitude,
                    altitude);
            }

            /*
            MAV_CMD_NAV_LOITER_TURNS	Loiter around this MISSION for X turns
            Mission Param #1	Turns
            Mission Param #2	Empty
            Mission Param #3	Radius around MISSION, in meters. If positive loiter clockwise, else counter-clockwise
            Mission Param #4	Desired yaw angle.
            Mission Param #5	Latitude
            Mission Param #6	Longitude
            Mission Param #7	Altitude
            */

            public CommandAck loiterTurns(Int32 numTurns, Int32 radiusMeters, Int32 latitude, Int32 longitude, Int32 altitude)
            {
                return runCommand(MAVLink.MAV_CMD.LOITER_TURNS, numTurns, default(Int32),
                    radiusMeters, default(Int32), latitude, longitude,
                    altitude);
            }

            /*
            19	MAV_CMD_NAV_LOITER_TIME	Loiter around this MISSION for X seconds
            Mission Param #1	Seconds (decimal)
            Mission Param #2	Empty
            Mission Param #3	Radius around MISSION, in meters. If positive loiter clockwise, else counter-clockwise
            Mission Param #4	Desired yaw angle.
            Mission Param #5	Latitude
            Mission Param #6	Longitude
            Mission Param #7	Altitude
            */

            public CommandAck loiterTime(Int32 loiterTimeSeconds, Int32 radiusMeters, Int32 latitude, Int32 longitude, Int32 altitude)
            {
                return runCommand(MAVLink.MAV_CMD.LOITER_TURNS,
                loiterTimeSeconds,default(Int32),
                radiusMeters, default(Int32), latitude, longitude,
                altitude);
            }

            /*
              20	MAV_CMD_NAV_RETURN_TO_LAUNCH	Return to launch location
              Mission Param #1	Empty
              Mission Param #2	Empty
              Mission Param #3	Empty
              Mission Param #4	Empty
              Mission Param #5	Empty
              Mission Param #6	Empty
              Mission Param #7	Empty
              */

            public CommandAck returnToLaunch()
            {
                return runCommand(MAVLink.MAV_CMD.RETURN_TO_LAUNCH, default(Int32), default(Int32),
                    default(Int32), default(Int32), default(Int32), default(Int32),
                    default(Int32));
            }

            private Object commandLock = new Object();
            private CommandAck runCommand(MAVLink.MAV_CMD command, Int32 param1,
                Int32 param2,
                Int32 param3,
                Int32 param4,
                Int32 param5,
                Int32 param6,
                Int32 param7)
            {
                lock (commandLock)
                {
                    logger.Debug("Running {0} command.", command);
                    connection.sendCommand((ushort)command, param1, param2,
                        param3, param4, param5, param6,
                        param7);
                    return fetchCommandAck(command);
                }
            }

        }
    }
}
