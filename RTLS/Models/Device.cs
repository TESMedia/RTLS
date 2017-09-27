using RTLS.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTLS.Models
{

    public class Device
    {
        public int Id { get; set; }

        public string MacAddress { get; set; }

        public DeviceStatus status { get; set; }

        public bool IsCreatedByAdmin { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public bool IsDisplay { get; set; }

        [NotMapped]
        public string StrStatus { get; set; }

        public int ? RtlsConfigureId { get; set; }

        [ForeignKey("RtlsConfigureId")]
        public virtual RtlsConfiguration RtlsConfiguration { get; set; }

    }

}