using RTLS.Domins;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RTLS.Domains
{
    public class RtlsConfiguration
    {
        public RtlsConfiguration()
        {
            Devices = new HashSet<Device>();
            SiteFloors = new HashSet<SiteFloor>();
        }

        [Key]
        public int SiteId { get; set; }

        [MaxLength(50)]
        [Required()]
        public string EngageSiteName { get; set; }

        [MaxLength(50)]
        [Required()]
        public string EngageBuildingName { get; set; }

        [MaxLength(200)]
        [Required()]
        public string EngageBaseAddressUri { get; set; }

        public int Port { get; set; }

        public virtual ICollection<Device> Devices { get; set; }

        public virtual ICollection<SiteFloor> SiteFloors { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }
    }
}