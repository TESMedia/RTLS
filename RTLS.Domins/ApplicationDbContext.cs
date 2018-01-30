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

        public DbSet<Site> Site { get; set; }
        public DbSet<RtlsConfiguration> RtlsConfiguration { get; set; }
        public DbSet<SiteFloor> SiteFloor { get; set; }

        public DbSet<Device> Device { get; set; }
        public DbSet<DeviceAssociateSite> DeviceAssociateSite { get; set; }

        public DbSet<LocationData> LocationData { get; set; }
        public DbSet<TrackMember> TrackMember { get; set; }
        public DbSet<AppLog> AppLogs { get; set; }
        public DbSet<TrackMacNotification> TrackMacNotification { get; set; }
        

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Site>().ToTable("Site");
            modelBuilder.Entity<RtlsConfiguration>().ToTable("RtlsConfiguration");
            modelBuilder.Entity<SiteFloor>().ToTable("SiteFloor");
            modelBuilder.Entity<Device>().ToTable("Device");
            modelBuilder.Entity<DeviceAssociateSite>().ToTable("DeviceAssociateSite");
            modelBuilder.Entity<LocationData>().ToTable("LocationData");
            modelBuilder.Entity<AppLog>().ToTable("AppLog");
            modelBuilder.Entity<TrackMacNotification>().ToTable("TrackMacNotification");
            modelBuilder.Entity<TrackMember>().ToTable("TrackMember");

            modelBuilder.Entity<Site>()
           .HasOptional(f => f.RtlsConfiguration)
           .WithRequired(s => s.Site);
        }
    }
}