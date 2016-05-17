using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Commands
{
    /*
       ///<summary> These defines are predefined OR-combined mode flags. There is no need to use values from this enum, but it                simplifies the use of the mode flags. Note that manual input is enabled in all modes as a safety override. </summary>
        public enum MAV_MODE
        {
    	///<summary> System is not ready to fly, booting, calibrating, etc. No flag is set. | </summary>
            PREFLIGHT=0, 
        	///<summary> System is allowed to be active, under manual (RC) control, no stabilization | </summary>
            MANUAL_DISARMED=64, 
        	///<summary> UNDEFINED mode. This solely depends on the autopilot - use with caution, intended for developers only. | </summary>
            TEST_DISARMED=66, 
        	///<summary> System is allowed to be active, under assisted RC control. | </summary>
            STABILIZE_DISARMED=80, 
        	///<summary> System is allowed to be active, under autonomous control, manual setpoint | </summary>
            GUIDED_DISARMED=88, 
        	///<summary> System is allowed to be active, under autonomous control and navigation (the trajectory is decided onboard and not pre-programmed by MISSIONs) | </summary>
            AUTO_DISARMED=92, 
        	///<summary> System is allowed to be active, under manual (RC) control, no stabilization | </summary>
            MANUAL_ARMED=192, 
        	///<summary> UNDEFINED mode. This solely depends on the autopilot - use with caution, intended for developers only. | </summary>
            TEST_ARMED=194, 
        	///<summary> System is allowed to be active, under assisted RC control. | </summary>
            STABILIZE_ARMED=208, 
        	///<summary> System is allowed to be active, under autonomous control, manual setpoint | </summary>
            GUIDED_ARMED=216, 
        	///<summary> System is allowed to be active, under autonomous control and navigation (the trajectory is decided onboard and not pre-programmed by MISSIONs) | </summary>
            AUTO_ARMED=220, 
        	///<summary>  | </summary>
            ENUM_END=221, 
        
        };
        
        */
    public class SetModeAction
    {
        public string mode { get; set; }
    }
}
