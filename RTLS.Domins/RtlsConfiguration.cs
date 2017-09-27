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
        }
        [Key]
        public int RtlsConfigurationId { get; set; }

        [MaxLength(50)]
        [Required()]
        public string EngageSiteName { get; set; }

        [MaxLength(50)]
        [Required()]
        public string EngageBuildingName { get; set; }

        [MaxLength(200)]
        [Required()]
        public string EngageBaseAddressUri { get; set; }

        [Required()]
        public int SiteId { get; set; }

        [Required()]
        public string SiteName { get; set; }

        public virtual ICollection<Device> Devices { get; set; }

    }
}