using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneParameterReference
{
    public class ParameterMetadataEntry
    {
        public String key { get; set; }
        public String DisplayName { get; set; }
        public String Description { get; set; }
        public String Units { get; set; }
        public String Upper { get; set; }
        public String Lower { get; set; }
        public String Increment { get; set; }
        public String User { get; set; }
    }
}
