using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Library.Aplication.Services
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:Port"]);
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var password = _configuration["EmailSettings:AppPassword"];

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.Credentials = new System.Net.NetworkCredential(senderEmail, password);
                client.EnableSsl = true;
                var mailMessage = new MailMessage(senderEmail, to, subject, body);
                client.Send(mailMessage);
            }
        }
    }
}
