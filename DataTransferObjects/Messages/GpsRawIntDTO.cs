using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class GpsRawIntDTO
    {
        public UInt64 time_usec { get; set; }
        public UInt16 fix_type { get; set; }
        public Int32 lat { get; set; }
        public Int32 lon { get; set; }
        public Int32 alt { get; set; }
        public UInt16 eph { get; set; }
        public UInt16 epv { get; set; }
        public UInt16 vel { get; set; }
        public UInt16 cog { get; set; }
        public UInt16 satellites_visible { get; set; }
        public String fixTypeEnum { get; set; }

    }
}