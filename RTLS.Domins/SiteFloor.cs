using RTLS.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins
{
    public class SiteFloor
    {
        public int Id { get; set; }

        [MaxLength(70)]
        public string SiteFloorName { get; set; }

        [MaxLength(70)]
        public string FlooeImagePath { get; set; }

        public int XRange { get; set; }

        public int YRange { get; set; }

        public int ScaleFactor { get; set; }

        [MaxLength(50)]
        public string  DisplayConfiguration { get; set; }

        public int RtlsConfigureId { get; set; } 

        [ForeignKey("RtlsConfigureId")]
        public virtual RtlsConfiguration RtlsConfiguration { get; set; }
    }
}
