using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins
{
    public class WifiUser
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please send UserName")]
        [MaxLength(300)]
        [Index(IsUnique = true)]
        public string UserName { get; set; }

        [MaxLength(300)]
        public string Email { get; set; }

        [MaxLength(300)]
        public string Password { get; set; }

        [MaxLength(300)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(300)]
        public string MobileNumer { get; set; }

        public int? GenderId { get; set; }

        public int? AgeId { get; set; }

        public int? GroupId { get; set; }

        //public int? SSId { get; set; }


        public DateTime? BirthDate { get; set; }

        public bool? promotional_email { get; set; }

        public bool? ThirdPartyOptIn { get; set; }

        public bool? UserOfDataOptIn { get; set; }

        public bool? AutoLogin { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }


        [MaxLength(50)]
        public string Custom1 { get; set; }

        [MaxLength(50)]
        public string Custom2 { get; set; }

        [MaxLength(50)]
        public string Custom3 { get; set; }

        [MaxLength(50)]
        public string Custom4 { get; set; }

        [MaxLength(50)]
        public string Custom5 { get; set; }

        [MaxLength(50)]
        public string Custom6 { get; set; }

        public string UniqueUserId { get; set; }

        public WifiUserCreateBy? WifiUserCreateBy { get; set; }

        //[ForeignKey("AgeId")]
        //public virtual Age Ages { get; set; }

        //[ForeignKey("GenderId")]
        //public virtual Gender Genders { get; set; }

        //[ForeignKey("GroupId")]
        //public virtual Group Group { get; set; }


        //[ForeignKey("SSId")]
        //[JsonIgnore]
        //public virtual NetWorkOfSite NetWorkOfSite { get; set; }
        public virtual UsersAddress UsersAddress { get; set; }
        //public virtual ICollection<UserUsagePackage> UserUsagePackages { get; set; }
        public virtual ICollection<WifiUserLoginCredential> WifiUserLoginCredential { get; set; }
       // public virtual ICollection<EventAttendee> EventAttendees { get; set; }

    }

    public enum WifiUserCreateBy
    {
        //Created By Admin
        Admin = 10,

        //Created through third Party Service
        External = 20,

        //Created through Internal CP Page
        Internal = 30
    }
}
