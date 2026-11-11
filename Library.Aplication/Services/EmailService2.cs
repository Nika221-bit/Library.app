using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Library.Application.Services
{
    public class EmailService
    {
        public static void SendEmail(string to, string subject, string body) /// todo appsettings.json
		{
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("ed1t0r22091@gmail.com", "vxnl qnzy tlaq maah");
            smtpClient.EnableSsl = true;


            MailMessage message = new MailMessage();
            message.From = new MailAddress("ed1t0r22091@gmail.com");
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;


            smtpClient.Send(message);
        }


        public void SendVerificationCode(string email, string verificationCode)
        {
            SendEmail(email, "Verification Code", $"Your verification code is: {verificationCode}");
        }
    }
}