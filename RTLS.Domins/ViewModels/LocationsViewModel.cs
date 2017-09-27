using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTLS.ReturnModel
{
    [Serializable]
    public class StatusReturn
    {
        public int returncode { get; set; }
        public string errmsg { get; set; }
    }

    public class ReturnNew
    {
        public StatusReturn result { get; set; }
    }


    [Serializable]
    public class Record
    {
        public string mac { get; set; }

        public string active_count { get; set; }

        public string sn { get; set; }

        public string bn { get; set; }

        public string fn { get; set; }
        public int x { get; set; }

        public int y { get; set; }

        public int z { get; set; }

        public string ts_last_seen { get; set; }

        public string[] an { get; set; }

    }

    [Serializable]
    public class LocationViewModel
    {
        public StatusReturn result;
        public List<Record> records;
        public LocationViewModel()
        {
            result = new StatusReturn();
            records = new List<Record>();
        }
    }
}