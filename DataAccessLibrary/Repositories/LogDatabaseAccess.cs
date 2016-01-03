namespace DataAccessLibrary.Repositories
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using DataAccessLibrary.Models;

    public partial class LogDatabaseAccess : DbContext
    {
        public LogDatabaseAccess()
            : base("name=LogRepository")
        {
        }

        public virtual DbSet<NLogEntity> NLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NLogEntity>()
                .Property(e => e.Level)
                .IsUnicode(false);
        }
    }
}
