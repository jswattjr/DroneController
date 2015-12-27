namespace DataAccessLibrary.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using DataAccessLibrary.Models;
    using System.Data.Entity.SqlServer;

    public class AdoDatabaseAccess : DbContext
    {
        // internal dll hack
        private static SqlProviderServices instance = SqlProviderServices.Instance;
   
        // Your context has been configured to use a 'AdoDatabaseAccess' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DataAccessLibrary.Repositories.DroneEntity' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'AdoDatabaseAccess' 
        // connection string in the application configuration file.
        public AdoDatabaseAccess()
            : base("name=AdoDatabaseAccess")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<DroneEntity> DroneEntities { get; set; }
    }
}