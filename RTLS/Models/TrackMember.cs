using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RTLS.Models
{
    public class TrackMember
    {
        [Key()]
        public int Id { get; set; }

        public string MacAddress { get; set; }

        public DateTime VisitedDateTime { get; set; }

        public DateTime PostDateTime { get; set; }

        public DateTime RecieveDateTime { get; set; }

        public string AreaName { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

    }
}