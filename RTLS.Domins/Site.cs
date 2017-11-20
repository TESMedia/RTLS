using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace RTLS.Domains
{
    public class Site
    {
        public Site()
        {
            //AdminSiteAcess = new HashSet<AdminSiteAccess>();
        }
        [Key]
        public int SiteId { get; set; }

        //[Index(IsUnique = true)]
        [StringLength(100)]
        
        public string SiteName { get; set; }
        // Foreign key 
        //public int UserId { get; set; }
       
        public int? CompanyId { get; set; }
        [MaxLength(200)]
        public string ControllerIpAddress { get; set; }
        [MaxLength(200)]
        public string MySqlIpAddress { get; set; }
        [MaxLength(200)]
        public string RtlsUrl { get; set; }
        [MaxLength(200)]
        public string DashboardUrl { get; set; }
        [MaxLength(200)]
        public string CpUrl { get; set; }
        [MaxLength(100)]
        public string RtlsPublisherUrl { get; set; }
        [MaxLength(100)]
        public string RtlsWebSocketUrl { get; set; }

        public virtual RtlsConfiguration RtlsConfiguration { get; set; }
        public ICollection<DeviceAssociateSite> DeviceAssociateSite { get; set; }
    }
}