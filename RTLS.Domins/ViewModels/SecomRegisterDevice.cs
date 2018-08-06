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
        public string type { get; set; }
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
            public string type { get; set; }
        }

        public class User
        {
            public string type { get; set; }
            public string sub_type { get; set; }
            public string part_code { get; set; }
            public string label { get; set; }
            public string icon { get; set; }
            public string color { get; set; }
        }

        public class BlackList
        {
            public bool status { get; set; }
            public string action { get; set; }
        }

      }
    
}
