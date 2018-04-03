using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace AppstreamLib.Utilities
{
    public class Email
    {
        public static void SendEmail(string destination_email, string content , string subject , bool isEnableSSL = false , bool isBodyHtml = false)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = ConfigurationManager.AppSettings["Email.Host"];
            smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["Email.Port"]);
            smtpClient.EnableSsl = isEnableSSL;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Email.Sender"], ConfigurationManager.AppSettings["Email.Sender.Password"]);
            System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
            email.From = new MailAddress(ConfigurationManager.AppSettings["Email.Sender"], ConfigurationManager.AppSettings["Email.Sender.Name"]);
            email.To.Add(destination_email);
            email.Subject = subject;
            email.IsBodyHtml = isBodyHtml;
            email.Body += content;

            try
            {
                smtpClient.Send(email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }


}
