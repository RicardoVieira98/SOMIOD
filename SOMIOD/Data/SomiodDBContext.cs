using SOMIOD.Library.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.Configuration;

namespace SOMIOD.Data
{
    public interface ISomiodDBContext
    {
        void OnModelCreating(DbModelBuilder modelBuilder);
    }
    public class SomiodDBContext : DbContext
    {
        private static readonly string _connectionString = WebConfigurationManager.ConnectionStrings["LocalInstanceBD"].ConnectionString;
        public SomiodDBContext() : base(_connectionString)
        {
        }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Library.Models.Data> Datas { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}