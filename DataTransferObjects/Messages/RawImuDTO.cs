using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class RawImuDTO
    {
        public Int16 xacc { get; set; }
        public Int16 yacc { get; set; }
        public Int16 zacc { get; set; }
        public Int16 xgyro { get; set; }
        public Int16 ygyro { get; set; }
        public Int16 zgyro { get; set; }
        public Int16 xmag { get; set; }
        public Int16 ymag { get; set; }
        public Int16 zmag { get; set; }
    }
}