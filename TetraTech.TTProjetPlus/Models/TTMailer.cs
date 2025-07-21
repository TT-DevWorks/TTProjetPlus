using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace TetraTech.TTProjetPlus.Models
{
    public class TTMailer
    {
        public static string TTmailUsername { get; set; }
        public static string TTmailPassword { get; set; }
        public static string TTmailHost { get; set; }
        public static int TTmailPort { get; set; }
        public static bool TTmailSSL { get; set; }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        static TTMailer()
        {
            TTmailHost = "smtp-mail.outlook.com";
            TTmailPort = 587; // TTmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
            TTmailSSL = true;
        }

        public void Send()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = TTmailHost;
            smtp.Port = TTmailPort;
            smtp.EnableSsl = TTmailSSL;
            smtp.Timeout = 5000000;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(TTmailUsername, TTmailPassword);

            using (var message = new MailMessage(TTmailUsername, ToEmail))
            {
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = IsHtml;
                smtp.Send(message);
            }
        }
    }
}