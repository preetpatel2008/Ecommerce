using Entity;

using System.Data.Entity;
using System;

namespace Repository.Services.Context
{
    public class AddDbContext : DbContext
    {
    
        public AddDbContext(string ConnectionStringName) : base(ConnectionStringName)
        {   
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AddDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }

    }
}
