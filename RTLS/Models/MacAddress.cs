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

        public int ? CompnayId { get; set; }

        [ForeignKey("CompnayId")]
        public virtual Company Company { get; set; }

    }

}