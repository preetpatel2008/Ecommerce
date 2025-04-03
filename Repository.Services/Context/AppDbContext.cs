using Entity;

using System.Data.Entity;
using System;

namespace Repository.Services.Context
{
    public class AppDbContext : DbContext
    {
    
        public AppDbContext(string ConnectionStringName) : base(ConnectionStringName)
        {   
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AppDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }

    }
}
