using RTLS.Domins;
using RTLS.Domins.Enums;
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
        public string EngageSiteName { get; set; }

        [MaxLength(50)]
        public string EngageBuildingName { get; set; }

        [MaxLength(200)]
        public string EngageBaseAddressUri { get; set; }

        public string OmniBaseAddressUri { get; set; }

        public int Port { get; set; }

        public int ApproachNotification { get; set; }

        public int AreaNotification { get; set; }

        public int? TimeFrame { get; set; }

        [MaxLength(200)]
        public string EndPointUrl { get; set; }

        public virtual ICollection<Device> Devices { get; set; }

        public virtual ICollection<SiteFloor> SiteFloors { get; set; }

        public virtual ICollection<RtlsArea> RtlsAreas { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

        [NotMapped]
        [EnumDataType(typeof(RtlsEngine))]
        public RtlsEngine RtlsEngineType { get; set; }

        public virtual int RtlsEngineTypeId
        {
            get
            {
                return (int)this.RtlsEngineType;
            }
            set
            {
                RtlsEngineType = (RtlsEngine)value;
            }
        }
    }
}