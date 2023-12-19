using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SOMIOD.Data
{
    public class SomiodDBContext : DbContext
    {
        private static readonly string _connectionString = WebConfigurationManager.ConnectionStrings["LocalInstanceBD"].ConnectionString;
        public SomiodDBContext() : base(_connectionString)
        {
        }
        public DbSet<Application> Applications { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}