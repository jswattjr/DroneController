using DroneConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DroneManager.Models.MessageContainers
{
    /*
    The general system state. If the system is following the MAVLink standard, the system state is mainly
    defined by three orthogonal states/modes: The system mode, which is either LOCKED
    (motors shut down and locked), MANUAL (system under RC control),
    GUIDED (system with autonomous position control, position setpoint controlled manually) or
    AUTO (system guided by path/waypoint planner). 
    The NAV_MODE defined the current flight state: LIFTOFF (often an open-loop maneuver), 
    LANDING, WAYPOINTS or VECTOR. This represents the internal navigation state machine. 
    The system status shows wether the system is currently active or not and if an emergency occured. 
    During the CRITICAL and EMERGENCY states the MAV is still considered to be active, but should start 
    emergency procedures autonomously. After a failure occured it should first move from active to 
    critical to allow manual intervention and then move to emergency after a certain timeout.

        
    onboard_control_sensors_present	uint32_t	Bitmask showing which onboard controllers and sensors are present. Value of 0: not present. Value of 1: present. Indices defined by ENUM MAV_SYS_STATUS_SENSOR
    onboard_control_sensors_enabled	uint32_t	Bitmask showing which onboard controllers and sensors are enabled: Value of 0: not enabled. Value of 1: enabled. Indices defined by ENUM MAV_SYS_STATUS_SENSOR
    onboard_control_sensors_health	uint32_t	Bitmask showing which onboard controllers and sensors are operational or have an error: Value of 0: not enabled. Value of 1: enabled. Indices defined by ENUM MAV_SYS_STATUS_SENSOR
    load	uint16_t	Maximum usage in percent of the mainloop time, (0%: 0, 100%: 1000) should be always below 1000
    voltage_battery	uint16_t	Battery voltage, in millivolts (1 = 1 millivolt)
    current_battery	int16_t	Battery current, in 10*milliamperes (1 = 10 milliampere), -1: autopilot does not measure the current
    battery_remaining	int8_t	Remaining battery energy: (0%: 0, 100%: 100), -1: autopilot estimate the remaining battery
    drop_rate_comm	uint16_t	Communication drops in percent, (0%: 0, 100%: 10'000), (UART, I2C, SPI, CAN), dropped packets on all links (packets that were corrupted on reception on the MAV)
    errors_comm	uint16_t	Communication errors (UART, I2C, SPI, CAN), dropped packets on all links (packets that were corrupted on reception on the MAV)
    errors_count1	uint16_t	Autopilot-specific errors
    errors_count2	uint16_t	Autopilot-specific errors
    errors_count3	uint16_t	Autopilot-specific errors
    errors_count4	uint16_t	Autopilot-specific errors
    */

    public class SystemStatus : MessageContainerBase
    {
        public List<MAVLink.MAV_SYS_STATUS_SENSOR> sensorsPresent = new List<MAVLink.MAV_SYS_STATUS_SENSOR>();
        public List<MAVLink.MAV_SYS_STATUS_SENSOR> sensorsEnabled = new List<MAVLink.MAV_SYS_STATUS_SENSOR>();
        public List<MAVLink.MAV_SYS_STATUS_SENSOR> sensorsHealth = new List<MAVLink.MAV_SYS_STATUS_SENSOR>();
        public UInt16 voltage_battery;
        public Int16 current_battery;
        public Int16 battery_remaining;
        public UInt16 drop_rate_comm;
        public UInt16 errors_comm;
        public UInt16 errors_count1;
        public UInt16 errors_count2;
        public UInt16 errors_count3;
        public UInt16 errors_count4;

        public override MAVLink.MAVLINK_MSG_ID MessageID
        {
            get
            {
                return MAVLink.MAVLINK_MSG_ID.SYS_STATUS;
            }
        }

        public override string DesctiptionUrl
        {
            get
            {
                return "https://pixhawk.ethz.ch/mavlink/#SYS_STATUS";
            }
        }

        public override Type getStructType()
        {
            return typeof(MAVLink.mavlink_sys_status_t);
        }

        public SystemStatus(MavLinkMessage message) : base(null)
        {
            if (message.messid == this.MessageID)
            {
                MAVLink.mavlink_sys_status_t raw_data = (MAVLink.mavlink_sys_status_t)message.data_struct;
                this.voltage_battery = raw_data.voltage_battery;
                this.current_battery = raw_data.current_battery;
                this.battery_remaining = raw_data.battery_remaining;
                this.drop_rate_comm = raw_data.drop_rate_comm;
                this.errors_comm = raw_data.errors_comm;
                this.errors_count1 = raw_data.errors_count1;
                this.errors_count2 = raw_data.errors_count2;
                this.errors_count3 = raw_data.errors_count3;
                this.errors_count4 = raw_data.errors_count4;

                // parse bit masks into lists
                IEnumerable<MAVLink.MAV_SYS_STATUS_SENSOR> values = EnumValues.GetValues<MAVLink.MAV_SYS_STATUS_SENSOR>();
                foreach (MAVLink.MAV_SYS_STATUS_SENSOR sensor in values)
                {
                    uint sensorMask = (uint)sensor;
                    if ((sensorMask & raw_data.onboard_control_sensors_enabled) == sensorMask)
                    {
                        sensorsEnabled.Add(sensor);
                    }
                    if ((sensorMask & raw_data.onboard_control_sensors_health) == sensorMask)
                    {
                        sensorsHealth.Add(sensor);
                    }
                    if ((sensorMask & raw_data.onboard_control_sensors_present) == sensorMask)
                    {
                        sensorsPresent.Add(sensor);
                    }
                }

            }
        }
    }
}
