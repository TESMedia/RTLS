using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins.ViewModels
{
    public class RtlsAreaViewModel
    {
        public int SiteId { get; set; }

        public string SiteName { get; set; }

        public int ApproachNotification { get; set; }

        public int AreaNotification { get; set; }

        public List<RtlsArea> GeoFencedAreas { get; set; }
    }
}
