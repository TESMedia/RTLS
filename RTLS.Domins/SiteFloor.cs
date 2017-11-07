using Newtonsoft.Json;
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

        public int XRangeFeedData { get; set; }

        public int YRangeFeedData{ get; set; }

        public int ScaleFactor { get; set; }

        public int CanvasXLength { get; set; }

        public int CanvasYLength { get; set; }

        public int TopStyle { get; set; }

        public int LeftStyle { get; set; }

        public int ImageXLength { get; set; }

        public int ImageYLength { get; set; }

        public int RtlsConfigureId { get; set; } 

        [ForeignKey("RtlsConfigureId")]

        [JsonIgnore]
        public virtual RtlsConfiguration RtlsConfiguration { get; set; }
    }
}
