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
using PPOK.Domain.Types;

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
            //the template needs to be gathered from db specified by a type

            Assembly domain = Assembly.GetExecutingAssembly();
            Func<string, StreamReader> ResourceStream = resource => new StreamReader(domain.GetManifestResourceStream(resource));
            using (var input = ResourceStream("PPOK.Domain.App_Data.RefillPrescriptionEmail.html"))
                SendReminderEmail = input.ReadToEnd();

            //this should change back to the original

//            SendReminderEmail = @"<body>
//    Your Prescription due on {date} is up!
//</body>";

        }

        private readonly EmailService emailService;

        public SendEmailService()
        {
            emailService = new EmailService(BotEmail, BotPassword);
        }

        public void Create(string toEmail, DateTime date) //change this to accept a prescription, make sure this gets the type of email
        {
            Prescription p = new Prescription();
            Patient pat = new Patient();
            Pharmacy pharm = new Pharmacy();
            FillHistory f = new FillHistory();
            pharm.Phone = "8675309";
            pharm.Name = "Bill and Ted's Excellent Pharmacy";
            pat.FirstName = "CAAARRRLLL";
            f.Date = DateTime.Now;

            
            pat.Pharmacy = pharm;
            p.Patient = pat;
            p.Fills = new List<FillHistory>() {
               f
            };
            
            
            emailService.SendEmail(
                from: BotEmail,
                to: toEmail,
                subject: "Prescription Refill",
                body: Util.NamedFormat(SendReminderEmail, p) //update the email using correct syntax to fill this stuff in
            );
        }
    }
}