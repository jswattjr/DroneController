using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Models
{
    public class DroneEntity : IEntity
    {
        [Key, Column(Order = 0)]
        public Guid id { get; set; }

        [Key, Column(Order = 1)]
        public String name { get; set; }
  
        public void Set(IEntity entity)
        {
            this.copy(entity as DroneEntity);
        }

        public void copy(DroneEntity entity)
        {
            // copy all settings from entity to this entity;
        }
    }
}
