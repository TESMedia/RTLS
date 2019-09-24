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
        //public RequestOmniModel()
        //{
        //    this.MacAddress = this.MacAddress.ToLower();
        //}

        //public string MacAddress { get; set; }
        private string _MacAddress;
        public string MacAddress
        {
            get
            {
                if (string.IsNullOrEmpty(_MacAddress))
                {
                    return _MacAddress;
                }
                return _MacAddress.ToLower();
            }
            set
            {
                _MacAddress = value;
            }
        }

        public int NotificationTypeId { get; set; }

        public int SiteId { get; set; }

        public string UserName { get; set; }
    }
}
