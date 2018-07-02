using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins.Identity
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class A8AdminDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, int,
        ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public A8AdminDbContext()
        : base("A8AdminModelEntity")
        {
            this.Configuration.ValidateOnSaveEnabled = false;
            //Database.CreateIfNotExists();
        }
        public static A8AdminDbContext Create()
        {
            return new A8AdminDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "dbo");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles", "dbo");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRoles", "dbo");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaims", "dbo");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogins", "dbo");
        }
    }
}
