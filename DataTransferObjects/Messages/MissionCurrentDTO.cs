using DroneManager.Models.MessageContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class MissionCurrentDTO
    {
        /// <summary> Sequence </summary>
        public UInt16 seq { get; set; }

        public MissionCurrentDTO( MissionCurrent source)
        {
            Utilities.CopySimilar.CopyAll(source, this);
        }
    }
}