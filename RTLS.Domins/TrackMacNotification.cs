using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RTLS.Domins
{
    public class TrackMacNotification
    {
        [Key()]
        public string MacAddress { get; set; }

        public DateTime LastVisitDateTime { get; set; }

        public DateTime? LastNotifiedDateTime { get; set; }

        public string SiteName { get; set; }
    }
}
