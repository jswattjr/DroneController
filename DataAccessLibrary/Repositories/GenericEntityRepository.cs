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
        AdoDatabaseAccess dbContext;
        System.Data.Entity.DbSet<T> dbAccess;

        public GenericEntityRepository(System.Data.Entity.DbSet<T> dbAccess, AdoDatabaseAccess dbContext)
        {
            this.dbContext = dbContext;
            this.dbAccess = dbAccess;
        }

        public T create(T entity)
        {
            entity.id = Guid.NewGuid();
            dbAccess.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public void delete(T entity)
        {
            T target = getById(entity.id);
            if (null != target)
            {
                dbAccess.Remove(target);
                dbContext.SaveChanges();
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
            T target = dbAccess.Where(b => b.name == name).FirstOrDefault();
            return target;
        }

        public T set(T entity)
        {
            T target = getById(entity.id);
            target.Set(entity);
            dbContext.SaveChanges();
            return target;
        }
    }
}
