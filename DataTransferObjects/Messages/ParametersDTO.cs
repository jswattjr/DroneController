using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObjects.Messages
{
    public class ParametersDTO
    {
        public int count { get; set; }

        public Dictionary<String, ParamValueDTO> parameters { get; set; }
        
    }
}