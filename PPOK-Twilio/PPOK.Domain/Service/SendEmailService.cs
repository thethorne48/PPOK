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
    public class ManageAwardService
    {
        //these will be the strings to hold the html for any emails that need to be sent
        private static readonly string SendReminderEmail;
        static ManageAwardService()
        {
            Assembly domain = Assembly.GetExecutingAssembly();
            Func<string, StreamReader> ResourceStream = resource => new StreamReader(domain.GetManifestResourceStream(resource));
            //each of these will correspond to each above variable that reads the file that holds the html
            using (var input = ResourceStream("PPOK.Domain.App_Data.RefillPrescriptionEmail.html"))
                SendReminderEmail = input.ReadToEnd();
        }

        private readonly EmailService emailService;

        public ManageAwardService()
        {
            emailService = new EmailService(BotEmail, BotPassword);
        }

        public void Create(string toEmail, string fromEmail)
        {

            emailService.SendEmail(
                from: "PPOK Twilio",
                to: "miller.matthew64@gmail.com",
                subject: "Prescription Refill",
                body: Util.NamedFormat(SendReminderEmail, new { name = "Tacos" }) //award needs to be an object to drop inside the {} of email
            );
        }
    }
}