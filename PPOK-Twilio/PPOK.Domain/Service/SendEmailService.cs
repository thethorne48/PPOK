using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using PPOK.Domain.Utility;
using Dapper;
using System.Dynamic;
using static PPOK.Domain.Utility.Config;

/// <summary>
/// This is a template to show how to create an email
/// </summary>
namespace PPOK.Domain.Service
{
    public class SendEmailService
    {
        //these will be the strings to hold the html for any emails that need to be sent
        private static readonly string SendReminderEmail;
        static SendEmailService()
        {
            //Assembly domain = Assembly.GetExecutingAssembly();
            //Func<string, StreamReader> ResourceStream = resource => new StreamReader(domain.GetManifestResourceStream(resource));
            //using (var input = ResourceStream("PPOK.Domain.App_Data.RefillPrescriptionEmail.html"))
            //    SendReminderEmail = input.ReadToEnd();

            //this should change back to the original

            SendReminderEmail = @"<body>
    Your Prescription due on {date} is up!
</body>";

        }

        private readonly EmailService emailService;

        public SendEmailService()
        {
            emailService = new EmailService(BotEmail, BotPassword);
        }

        public void Create(string toEmail)
        {
            emailService.SendEmail(
                from: BotEmail,
                to: toEmail,
                subject: "Prescription Refill",
                body: Util.NamedFormat(SendReminderEmail, new { date = DateTime.Now }) // needs to be an object to drop inside the {} of email
            );
        }
    }
}