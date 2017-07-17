using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTLS.Models
{
    public class Company
    {
        [Key()]
        public int CompnayId { get; set; }

        public string CompanyName { get; set; }
    }
}