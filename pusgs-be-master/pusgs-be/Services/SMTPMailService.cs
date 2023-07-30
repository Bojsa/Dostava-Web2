using Microsoft.Extensions.Options;
using System;
using pusgs_be.Interfaces;
using pusgs_be.Models;
using System.Net.Mail;
using System.Text;

namespace pusgs_be.Services
{
    public class SMTPMailService : ISMTPMail
    {
        private readonly MailSettings emailSettings;

        public SMTPMailService(IOptions<MailSettings> emailsettings)
        {
            this.emailSettings = emailsettings.Value;
        }

        public void SendMail(string to, string subject, string body)
        {
            MailMessage message = new MailMessage(emailSettings.From, to);
            message.Subject = subject;
            message.Body = body;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(emailSettings.Host, emailSettings.Port);
            System.Net.NetworkCredential basicCredential1 = new System.Net.NetworkCredential(emailSettings.Username, emailSettings.Password);
            client.EnableSsl = emailSettings.EnableSsl;
            client.UseDefaultCredentials = emailSettings.UseDefaultCredentials;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
