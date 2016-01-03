using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Models;

namespace DataAccessLibrary.Repositories
{
    public class LogRepository
    {
        LogDatabaseAccess dbContext;
        System.Data.Entity.DbSet<NLogEntity> dbAccess;

        public LogRepository(System.Data.Entity.DbSet<NLogEntity> dbAccess, LogDatabaseAccess dbContext)
        {
            this.dbContext = dbContext;
            this.dbAccess = dbAccess;
        }

        public void delete(NLogEntity entity)
        {
            NLogEntity target = getById(entity.ID);
            if (null != target)
            {
                dbAccess.Remove(target);
                dbContext.SaveChanges();
            }
        }

        public IEnumerable<NLogEntity> getAll()
        {
            LinkedList<NLogEntity> reorder = new LinkedList<NLogEntity>();
            foreach (NLogEntity entry in dbAccess)
            {
                reorder.AddFirst(entry);
            }
            return reorder;
        }

        public NLogEntity getById(int id)
        {
            NLogEntity target = dbAccess.Find(id);
            return target;
        }

    }
}
