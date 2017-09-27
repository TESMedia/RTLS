using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTLS.ReturnModel
{
    public class MonitorDevice
    {
        public string monitor_updated { get; set; }

        public string bn { get; set; }

        public string sn { get; set; }

        public string time_stamp { get; set; }

        public string device_id { get; set; }

    }

    [NotMapped]
    public class MonitorDevices
    {
        public MonitorDevices()
        {
            records = new List<MonitorDevice>();
        }
        public List<MonitorDevice> records { get; set; }
        public StatusReturn result;
    }



}