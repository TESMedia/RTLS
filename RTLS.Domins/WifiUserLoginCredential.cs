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
    public class WifiUserLoginCredential
    {
        public int Id { get; set; }

        public int WifiUserId { get; set; }

        [MaxLength(300)]
        public string UserName { get; set; }

        [MaxLength(300)]
        public string Password { get; set; }

        public int SiteId { get; set; }

        public int NetWorkOfSiteId { get; set; }

        public Nullable<double> VersionTermAndCond { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

        private DateTime _createdDateTime = DateTime.Now;
        public DateTime CreatedDateTime
        {
            get
            {
                return _createdDateTime;
            }
            set
            {
                _createdDateTime = value;
            }
        }

        public int? DeviceId { get; set; }

        public bool IsRegisterFromAssociateNetwork { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        [ForeignKey("NetWorkOfSiteId")]
        public virtual NetWorkOfSite NetWorkOfSite { get; set; }

        [ForeignKey("WifiUserId")]
        public virtual WifiUser WifiUser { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }
    }
}
