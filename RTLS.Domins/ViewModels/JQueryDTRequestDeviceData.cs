using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins.ViewModels
{
    public class JQueryDTRequestDeviceData
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

        public bool IsTrackByAdmin { get; set; }

        public int RtlsConfigurationId { get; set; }

        //-------------------Page Test---------------------
        
        
        public int RecordToDisply { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }

    }
}
