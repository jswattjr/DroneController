using DroneManager.Models.MessageContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class SystemTimeDTO
    {
        public UInt64 time_unix_sec { get; set; }
        public UInt32 time_boot_ms { get; set; }

        public SystemTimeDTO(SystemTime source)
        {
            Utilities.CopySimilar.CopyAll(source, this);
        }
    }
}