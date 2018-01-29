using RTLS.Domins.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domains
{
    public class DeviceAssociateSite
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public int SiteId { get; set; }

        public bool IsDeviceRegisterInRtls { get; set; }

        public bool IsRegisterInCaptivePortal { get; set; }

        public DeviceStatus status { get; set; }

        public bool IsTrackByRtls { get; set; }

        public bool IsTrackByAdmin { get; set; }

        public bool IsCreatedByAdmin { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

    }

    //public enum DeviceStatus
    //{
    //    None = 0,
    //    Registered = 1,
    //    Failed = -1,
    //    DeRegistered = 2
    //}
}
