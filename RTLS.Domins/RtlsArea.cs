using RTLS.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domains
{
    public class RtlsArea
    {
        [Key]
        public int Id { get; set; }

        public int RtlsConfigurationId { get; set; }

        [MaxLength(200)]
        public string GeoFencedAreas { get; set; }

        //[MaxLength(200)]
        //public string GeoFencedArea2 { get; set; }

        //[MaxLength(200)]
        //public string GeoFencedArea3 { get; set; }

        //[MaxLength(200)]
        //public string GeoFencedArea4 { get; set; }

        [ForeignKey("RtlsConfigurationId")]
        public virtual RtlsConfiguration RtlsConfiguration { get; set; }
    }
}
