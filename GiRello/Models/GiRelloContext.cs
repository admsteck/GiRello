using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GiRello.Models
{
    public class GiRelloContext : DbContext
    {
        public DbSet<Auth> Auths { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<GiRelloContext, GiRello.Migrations.Configuration>());
        }
    }
}