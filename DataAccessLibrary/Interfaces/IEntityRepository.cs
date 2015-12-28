using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Interfaces
{
    public interface IEntityRepository<T> where T : class, IEntity
    {
        IEnumerable<T> getAll();

        T getById(Guid id);

        T getByName(String name);

        T set(T entity);

        T create(T entity);

        void delete(T entity);
    }
}
