using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class DroneEntity
    {
        [Key]
        public Guid id { get; set; }

        public void Set(DroneEntity entity)
        {
            // copy all settings from entity to this entity;
        }
    }
}
