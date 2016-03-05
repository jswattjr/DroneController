using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneManager.Models
{
    public partial class Drone
    {
        public Commands Command
        {
            get
            {
                if (null == Command)
                {
                    Command = new Commands(this);
                }
                return Command;
            }
            private set
            {
                Command = value;
            }
        }

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
            MavLinkConnection connection;
            public Commands(Drone droneObj)
            {
                connection = droneObj.connection;
            }


            /*
            400	MAV_CMD_COMPONENT_ARM_DISARM	Arms / Disarms a component
            Mission Param #1	1 to arm, 0 to disarm
            */
            public void arm()
            {
                connection.sendCommand((ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM, Convert.ToInt32(true));
            }

            public void disarm()
            {
                connection.sendCommand((ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM, Convert.ToInt32(false));
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
            public void takeoff(Int32 heightInMeters)
            {
                connection.sendCommand((ushort)MAVLink.MAV_CMD.TAKEOFF, default(Int32), default(Int32),
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
            public void landAtLocation(Int32 latitude, Int32 longitude, Int32 altitude = 1)
            {
                connection.sendCommand((ushort)MAVLink.MAV_CMD.LAND, default(Int32), default(Int32),
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

            public void navigateWaypoint(Int32 latitude, Int32 longitude, Int32 altitude)
            {
                connection.sendCommand((ushort)MAVLink.MAV_CMD.WAYPOINT, default(Int32), default(Int32),
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

            public void loiterUnlimited(Int32 radiusMeters, Int32 latitude, Int32 longitude, Int32 altitude)
            {
                connection.sendCommand((ushort)MAVLink.MAV_CMD.LOITER_UNLIM, default(Int32), default(Int32),
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

            public void loiterTurns(Int32 numTurns, Int32 radiusMeters, Int32 latitude, Int32 longitude, Int32 altitude)
            {
                connection.sendCommand((ushort)MAVLink.MAV_CMD.LOITER_TURNS, numTurns, default(Int32),
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

            public void loiterTime(Int32 loiterTimeSeconds, Int32 radiusMeters, Int32 latitude, Int32 longitude, Int32 altitude)
            {
                connection.sendCommand((ushort)MAVLink.MAV_CMD.LOITER_TURNS,
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

            public void returnToLaunch()
            {
                connection.sendCommand((ushort)MAVLink.MAV_CMD.RETURN_TO_LAUNCH, default(Int32), default(Int32),
                    default(Int32), default(Int32), default(Int32), default(Int32),
                    default(Int32));
            }

        }
    }
}
