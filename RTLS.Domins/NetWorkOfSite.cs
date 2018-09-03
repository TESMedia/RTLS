using Newtonsoft.Json;
using RTLS.Domains;
using RTLS.Domins.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins
{
   public class NetWorkOfSite
    {
        //public NetWorkOfSite()
        //{
        //    WifiUsers = new HashSet<WifiUser>();
        //    //Forms = new Form();
        //}

        [Key()]
        public int NetWorkOfSiteId { get; set; }

        [MaxLength(200)]
        public string SsidName { get; set; }

        [MaxLength(200)]
        public string EssProfile { get; set; }

        [MaxLength(200)]
        public string Status { get; set; }

        [MaxLength(200)]
        public string SecurityProfile { get; set; }

        [MaxLength(200)]
        public string EssidType { get; set; }

        [MaxLength(200)]
        public string BroadCast { get; set; }

        [MaxLength(200)]
        public string BeSpokePageName { get; set; }

        [MaxLength(200)]
        public string Tunnel { get; set; }

        public Nullable<bool> AutoLogin { get; set; }

        public int SiteId { get; set; }

        //public int CaptivePortalConfigurationId { get; set; }

        public int? ControlTypeId { get; set; }

        public int? SubControlTypeId { get; set; }

        public bool IsInCaptivePortal { get; set; }

        //[DefaultValue(false)]
        //public bool ISBeSpokeAvailable { get; set; }
        public bool IsAssociateNetworkExist { get; set; }

        public Nullable<double> VersionTermAndCond { get; set; }

        [MaxLength]
        public string TermsAndCondDoc { get; set; }

        [MaxLength(500)]
        public string SsidLogo { get; set; }

        public DateTime? CreatedDateTime { get { return DateTime.Now; } set { } }

        [MaxLength(200)]
        public string SecurityKey { get; set; }

        //[ForeignKey("ControlTypeId")]
        //public ControlType ControlType { get; set; }

        //[ForeignKey("SubControlTypeId")]
        //public SubControlType SubControlType { get; set; }

        [ForeignKey("SiteId")]

        [JsonIgnore]

        //public CaptivePortalConfiguration CaptivePortalConfiguration { get; set; }

        public virtual Site Site { get; set; }

        //public virtual ICollection<WifiUser> WifiUsers { get; set; }

        //public virtual Form Forms { get; set; }

        //public virtual ICollection<SSIDPackage> SSIDPackages { get; set; }
        //public virtual ICollection<TermsAndCondition> TermsAndConditions { get; set; }
        //public virtual ICollection<AsssociateNetwork> AsssociateNetworks { get; set; }

        [NotMapped]
        [EnumDataType(typeof(LocationServicesType))]
        public LocationServicesType LocServiceType { get; set; }

        public virtual int? LocServiceTypeId
        {
            get
            {
                return (int)this.LocServiceType;
            }
            set
            {
                if (value.HasValue)
                {
                    LocServiceType = (LocationServicesType)value;
                }
            }
        }
    }
}
