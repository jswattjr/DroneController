using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Models
{
    public class SettingEntity : IEntity
    {
        [Key, Column(Order=0)]
        public Guid id { get; set; }

        [Key, Column(Order = 1)]
        public String name { get; set; }

        public void Set(IEntity entity)
        {
            SettingEntity setting = entity as SettingEntity;
            if ( null != setting)
            {
                key = setting.key;
                value = setting.value;
            }
            else
            {
                throw new ArgumentException("Entity passed into SettingEntity 'Set' function is not a SettingEntity");
            }
        }

        public String key { get; set; }
        public String value { get; set; }
    }
}
