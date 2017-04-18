using System;
using System.Net;
using System.Net.Mail;
using static System.Text.Encoding;
using static PPOK.Domain.Utility.Config;

namespace PPOK.Domain.Service
{
    public class EmailService : IDisposable
    {
        private readonly SmtpClient client;
        private readonly NetworkCredential credentials;
        public string Username
        {
            get { return credentials.UserName; }
            set { credentials.UserName = value; }
        }
        public string Password
        {
            get { return credentials.Password; }
            set { credentials.Password = value; }
        }

        public EmailService(string username, string password, string host = "smtp.gmail.com", int port = 587)
        {
            client = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = credentials = new NetworkCredential(username, password)
            };
        }

        public EmailService() : this(BotEmail, BotPassword)
        {
        }

        public void Dispose() { client.Dispose(); }

        public void SendEmail(string to, string subject, string body, bool bodyIsHTML = true)
        {
            SendEmail(BotEmail, to, subject, body, bodyIsHTML);
        }

        public void SendEmail(string from, string to, string subject, string body, bool bodyIsHTML = true)
        {
            MailAddress fromAddr = new MailAddress(from);
            MailAddress toAddr = new MailAddress(to);
            using (MailMessage message = new MailMessage(fromAddr, toAddr))
            {
                message.Subject = subject;
                message.SubjectEncoding = UTF8;
                message.Body = body;
                message.BodyEncoding = UTF8;
                message.IsBodyHtml = bodyIsHTML;
                client.Send(message);
            }
        }
    }
}