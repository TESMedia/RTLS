using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTLS.ViewModel
{
    public class RequestLocationDataVM
    {
        public string[] MacAddresses { get; set; }

        public string Mac { get; set; }

        public string CompanyName { get; set; }

        public int SiteId { get; set; }

        public string SiteName { get; set; }

        public string EngageSiteName { get; set; }

        public string EngageBuildingName { get; set; }

        public string EngageBaseAddressUri { get; set; }

        public bool IscreatedByAdmin { get; set; }

        public bool IsDisplay { get; set; }

        public int RtlsConfigurationId { get; set; }

    }
}