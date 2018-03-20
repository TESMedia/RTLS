using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;


namespace RTLS.Domins.Identity
{
    //[Validator(typeof(UserValidator))]
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUser<int>
    {

        public ApplicationUser()
        {
            CreationDate = DateTime.Now;
            UpdateDate = DateTime.Now;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser,int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity>
            GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        [Required()]
        public override string UserName { get; set; }

        public string ForeName { get; set; }

        public string LastName { get; set; }
        public override string Email { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime  UpdateDate { get; set; }

        public int? SiteId { get; set; }
      
        [MaxLength(50)]
        public string Status { get; set; }

    }
}