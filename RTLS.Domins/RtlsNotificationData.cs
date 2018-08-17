using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTLS.Domins
{
    public class RtlsNotificationData
    {
        public int Id { get; set; }

        public string MacAddress { get; set; }

        public string UserName { get; set; }

        public DateTime NotifyDateTime { get; set; } = DateTime.Now;

        public string NotificationSenderTimeStamp { get; set; }
    }
}
