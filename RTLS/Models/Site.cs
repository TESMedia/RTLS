using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTLS.Models
{
    public class Site
    {

        [Key()]
        public int SiteId { get; set; }
        public string SiteName { get; set; }
    }
}