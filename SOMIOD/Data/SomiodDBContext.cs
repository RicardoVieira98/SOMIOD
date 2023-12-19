using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SOMIOD.Data
{
    public class SomiodDBContext : DbContext
    {
        public SomiodDBContext(string connectionString) : base(connectionString)
        {
        }
        public DbSet<Application> Applications { get; set; }
        public DbSet<SOMIOD.Models.Container> Containers { get; set; }
        public DbSet<SOMIOD.Models.Data> Datas { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}