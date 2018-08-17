using Newtonsoft.Json;
using RTLS.Domins;
using RTLS.Domins.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTLS.Domains
{
    public class Device
    {
        [Key()]
        public int DeviceId { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "Please send MacAddress")]
        [Display(Name = "MacAddress")]
        //[Index(IsUnique = true)]
        //[RegularExpression("^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$",ErrorMessage ="MacAddress format not valid")]
        public string MacAddress { get; set; }
        
        [MaxLength(100)]
        public string BrowserName { get; set; }

        [MaxLength(100)]
        public string OperatingSystem { get; set; }

        public bool IsMobile { get; set; }

        [MaxLength(100)]
        public string UserAgentName { get; set; }

        public ICollection<DeviceAssociateSite> DeviceAssociateSite { get; set; }

        public virtual OmniDeviceMapping OmniDeviceMapping { get; set; }

    }

}