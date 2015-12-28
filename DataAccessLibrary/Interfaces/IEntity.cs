using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Interfaces
{
    public interface IEntity
    {
        Guid id { get; set; }

        String name { get; set; }

        void Set(IEntity entity);
    }
}
