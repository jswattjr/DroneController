using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneConnection
{
    /*
        Commands section of MavLink Connection. Contains outbound messages / user invoked messages
    */
    public partial class MavLinkConnection
    {
        public Commands Command {
            get {
                if (null == Command)
                {
                    Command = new Commands(this);
                }
                return Command;
            }
            private set {
                Command = value;
            }
        }

        public class Commands
        {
            MavLinkConnection parent;
            public Commands(MavLinkConnection connection)
            {
                parent = connection;
            }

            private void sendCommand(ushort commandType, params Int32[] parameters)
            {
                MAVLink.mavlink_command_long_t req = new MAVLink.mavlink_command_long_t();

                req.target_system = Convert.ToByte(parent.systemId);
                req.target_component = Convert.ToByte(parent.componentId);

                req.command = commandType;

                // this loop takes input parameters and assigns them to the correct part of the struct
                // not sure how to do this better without reflection, since none of these are nullable
                // and I don't want to set values unnecessarily
                for (int paramIndex = 0; paramIndex < parameters.Length; paramIndex++)
                {
                    Int32 parameter = parameters[paramIndex];
                    if (paramIndex == 1)
                    {
                        req.param1 = parameter;
                    }
                    if (paramIndex == 2)
                    {
                        req.param2 = parameter;
                    }
                    if (paramIndex == 3)
                    {
                        req.param3 = parameter;
                    }
                    if (paramIndex == 4)
                    {
                        req.param4 = parameter;
                    }
                    if (paramIndex == 5)
                    {
                        req.param5 = parameter;
                    }
                    if (paramIndex == 6)
                    {
                        req.param6 = parameter;
                    }
                    if (paramIndex == 7)
                    {
                        req.param7 = parameter;
                    }
                }


                byte[] packet = parent.mavlinkParse.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);

                parent.port.Write(packet, 0, packet.Length);

                /*
                // Implement Async Ack code?
                try
                {
                    var ack = readsomedata<MAVLink.mavlink_command_ack_t>();
                    if (ack.result == (byte)MAVLink.MAV_RESULT.ACCEPTED)
                    {

                    }
                }
                catch
                {
                }
                */
            }

            public void arm(Boolean armValue = true)
            {
                sendCommand((ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM, Convert.ToInt32(armValue));
            }
        }

    }
}
