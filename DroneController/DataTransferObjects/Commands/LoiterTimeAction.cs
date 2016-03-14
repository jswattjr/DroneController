using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneController.DataTransferObjects.Commands
{
    public class LoiterTimeAction : LocationBasedAction
    {
        public Int32 loiterTimeSeconds { get; set; }
        public Int32 radiusMeters { get; set; }
    }
}