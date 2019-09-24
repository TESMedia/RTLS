using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins.ViewModels
{
    public class CPUserGuestIdMapping
    {
        public CPUserGuestIdMapping()
        {
            user = new userinfo();
        }

        public userinfo user { get; set; }

        public string name { get; set; } = "";

        public string mac { get; set; } = "";

        public string type { get; set; } = "cp";

        public class userinfo
        {
            public string id { get; set; } = "";
        }
    }
}
