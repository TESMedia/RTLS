using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins.ViewModels
{
    public class EngageRegisterDevice
    {
        public string MacAddress { get; set; }
        public string EngageSiteName { get; set; }
        public string EngageBuildingName { get; set; }
        public string EngageBaseAddressUri { get; set; }

    }
}
