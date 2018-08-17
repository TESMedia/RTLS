using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins.ViewModels
{
   public class FilterLocationData
   {

        public string MacAddress { get; set; }

        public string AreaName { get; set; }
        public int RecordToDisply { get; set; }
        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int TotalPage { get; set; }
        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }
        public string SiteName { get; set; }
        public int SiteId { get; set; }
        public int TimeFrame { get; set; }
    }
}
