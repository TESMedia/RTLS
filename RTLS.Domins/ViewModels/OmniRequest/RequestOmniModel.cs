using RTLS.Domins.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins.ViewModels.OmniRequest
{
  public class RequestOmniModel
    {
        public string MacAddress { get; set; }

        public int NotificationTypeId { get; set; }

        public int SiteId { get; set; }

        public string UserName { get; set; } = "user 1";
    }
}
