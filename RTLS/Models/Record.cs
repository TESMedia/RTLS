using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Models
{
    public class Record
    {
      public LocationData [] records {get;set;}
    }

    public class ListOfArea
    {
        public Record device_notification { get; set; }
    }
}
