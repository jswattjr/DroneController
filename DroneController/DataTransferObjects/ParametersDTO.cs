using DroneManager.Models.MessageContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneController.DataTransferObjects
{
    public class ParametersDTO
    {
        public Dictionary<String, ParamValueDTO> parameters { get; set; }
        
        public ParametersDTO (Dictionary<String, ParamValue> source)
        {
            foreach(String key in source.Keys)
            {
                parameters.Add(key, new ParamValueDTO(source[key]));
            }
        }
    }
}