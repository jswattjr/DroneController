﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Commands
{
    public class LoiterUnlimitedAction : LocationBasedAction
    {
        public Int32 radiusMeters { get; set; }
    }
}