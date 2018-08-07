using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins.ViewModels
{
   public  class SecomRegisterDevice
    {
        public SecomRegisterDevice()
        {
            station_info = new StationInfo();

        }

        public string mac { get; set; }
        public string type { get; set; } = "station";
        public StationInfo station_info { get; set; }
        public class StationInfo
        {
            public StationInfo()
            {
                device = new Device();
                user = new User();
                black_list = new BlackList();
            }
            public Device device { get; set; }
            public User user { get; set; }
            public BlackList black_list { get; set; }
        }
        public class Device
        {
            public string id { get; set; }
            public string type { get; set; } = "";
        }

        public class User
        {
            public string type { get; set; } = "visitor";
            public string sub_type { get; set; } = "visitor";
            public string part_code { get; set; } = " ";
            public string label { get; set; } = "user 1";
            public string icon { get; set; } = "test.png";
            public string color { get; set; } = "";
        }

        public class BlackList
        {
            public bool status { get; set; } = true;
            public string action { get; set; } = "";
        }

      }
    
}
