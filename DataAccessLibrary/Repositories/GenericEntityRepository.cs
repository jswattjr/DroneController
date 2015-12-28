using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;

namespace DataAccessLibrary.Repositories
{
    class GenericEntityRepository<T> : IEntityRepository<T> where T : class, IEntity
    {
        System.Data.Entity.DbSet<T> dbAccess;

        public GenericEntityRepository(System.Data.Entity.DbSet<T> dbAccess){
            this.dbAccess = dbAccess;
        }

        public T create(T entity)
        {
            entity.id = Guid.NewGuid();
            dbAccess.Add(entity);
            return entity;
        }

        public void delete(T entity)
        {
            T target = getById(entity.id);
            if (null != target)
            {
                dbAccess.Remove(target);
            }
        }

        public IEnumerable<T> getAll()
        {
            return dbAccess;
        }

        public T getById(Guid id)
        {
            T target = dbAccess.Find(id);
            return target;
        }
        public T getByName(String name)
        {
            T target = dbAccess.Find(name);
            return target;
        }

        public T set(T entity)
        {
            T target = getById(entity.id);
            target.Set(entity);
            return target;
        }
    }
}
