using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Threading.Tasks;


namespace RTLS.Domins.Identity
{

    /// <summary>
    /// 
    /// </summary>
    public class EmailService : IIdentityMessageService
    {
       public Task SendAsync(IdentityMessage message)
        {
            string senderID = "tls@tes.media";
            using (SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.avecsys.net", // smtp server address here…
                Port = 25,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential("user@smtp.avecsys.net", "ema1ls3rv3r"),
                Timeout = 30000,
            })
            {
                try
                {
                    MailMessage mailMessage = new MailMessage(senderID, message.Destination, message.Subject, message.Body);
                    mailMessage.IsBodyHtml = true;
                    smtp.Send(mailMessage);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                return Task.FromResult(0);
            }              
        }
    }
}