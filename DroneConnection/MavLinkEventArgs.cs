using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneConnection
{
    public class MavLinkEventArgs : EventArgs
    {
        public MavLinkMessage message;

        public MavLinkEventArgs (MavLinkMessage message)
        {
            this.message = message;
        }
    }
}
