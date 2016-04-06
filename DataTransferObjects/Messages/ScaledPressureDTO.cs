using DroneManager.Models.MessageContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class ScaledPressureDTO
    {
        public UInt32 time_boot_ms { get; set; }
        public Single press_abs { get; set; }
        public Single press_diff { get; set; }
        public Int16 temperature { get; set; }

        public ScaledPressureDTO( ScaledPressure source)
        {
            Utilities.CopySimilar.CopyAll(source, this);
        }
    }
}