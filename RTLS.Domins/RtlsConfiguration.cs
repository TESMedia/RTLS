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
        [MaxLength(50)]
        public string SiteName { get; set; }

        public Displaytype DisplayType { get; set; }

        public int Port { get; set; }

        [MaxLength(100)]
        public string PulisherSocketHostAddress { get; set; }

        public virtual ICollection<Device> Devices { get; set; }

        public virtual ICollection<SiteFloor> SiteFloors { get; set; }
    }

    public enum Displaytype
    {
        singleScreen = 10,
        SplitScreen = 20
    }
}