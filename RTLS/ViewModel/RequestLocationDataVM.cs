using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTLS.ViewModel
{
    public class RequestLocationDataVM
    {
        public string[] MacAddresses { get; set; }

        public string CompanyName { get; set; }

        public string SiteName { get; set; }

        public bool IscreatedByAdmin { get; set; }
    }
}