using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTLS.Domins.ViewModels.Request
{
    public class NotificationRequest
    {
        [Required(ErrorMessage = "SiteId Missing")]
        public int SiteId { get; set; }
       // [Required(ErrorMessage = "SessionId Missing")]
        public string SessionId { get; set; }
        public string[] UserIdList { get; set; }
        public string[] MacAddressList { get; set; }
        [Required(ErrorMessage = "Missing Notification Type")]
        public NotificationType NotificationType { get; set; }
        [Required(ErrorMessage = "Missing Reset Time")]
        public int ResetTime { get; set; }
        public string[] AreaID { get; set; }
    }
    public enum NotificationType
    {
        Approach = 0, Entry = 1, All = 3
    }
}
