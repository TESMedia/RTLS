using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RTLS.Models
{
    public class MacAddress
    {
        public int Id { get; set; }

        public string Mac { get; set; }

        public int Intstatus { get; set; }

        public int ? SiteId { get; set; }

        public bool IsCreatedByAdmin { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site site { get; set; }
    }

}