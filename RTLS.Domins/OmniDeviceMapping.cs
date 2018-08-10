using RTLS.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins
{
    public class OmniDeviceMapping
    {
        [Key()]
        public int DeviceId { get; set; }

        public string UniqueId { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }
    }
}
