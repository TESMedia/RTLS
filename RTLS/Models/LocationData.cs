using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RTLS.Models
{
    public class LocationData
    {
        [Key]
        public int Id { get; set; }
        public string mac { get; set; }

        public string sequence { get; set; }

        public string sn { get; set; }

        public string bn { get; set; }

        public string fn { get; set; }

        public int x { get; set; }

        public int y { get; set; }

        public int z { get; set; }

        public string last_seen_ts { get; set; }

        public string action { get; set; }

        public string fix_result { get; set; }

        [NotMapped]
        public string[] an { get; set; }

        [NotMapped]
        public DateTime LastSeenDatetime { get; set; }

        [NotMapped]
        public DateTime PostDateTime { get; set; }

        public string AreaName { get; set; }

        public int? AreaId { get; set; }

        [ForeignKey("AreaId")]
        public virtual Area Areas { get; set; }
    }
}