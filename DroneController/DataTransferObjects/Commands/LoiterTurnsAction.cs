using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneController.DataTransferObjects.Commands
{
    public class LoiterTurnsAction : LocationBasedAction
    {
        public Int32 numTurns { get; set; }
        public Int32 radiusMeters { get; set; }
    }
}