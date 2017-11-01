using RTLS.Domins;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RTLS.Domains
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
            //Database.CreateIfNotExists();
        }
        public DbSet<Device> Device { get; set; }
        public DbSet<LocationData> LocationData { get; set; }
        public DbSet<TrackMember> CheckMembers { get; set; }
        public DbSet<AppLog> AppLogs { get; set; }
        public DbSet<RtlsConfiguration> RtlsConfigurations { get; set; }
        public DbSet<SiteFloor> SiteFloor { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}