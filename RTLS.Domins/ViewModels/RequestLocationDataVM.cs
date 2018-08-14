using RTLS.Domins.Enums;
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

        public string OmniBaseAddressUri { get; set; }

        public RtlsEngine RtlsEngineType { get; set; }

        public bool IscreatedByAdmin { get; set; }

        public bool IsDisplay { get; set; }

        public bool IsTrackByAdmin { get; set; }

        public bool IsEntryNotify { get; set; }

        public int RtlsConfigurationId { get; set; }

    }
}