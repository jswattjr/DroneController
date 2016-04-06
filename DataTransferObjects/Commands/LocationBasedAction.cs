using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Commands
{
    public class LocationBasedAction
    {
        public Int32 latitude { get; set; }
        public Int32 longitude { get; set; }
        public Int32 altitude { get; set; }
    }
}