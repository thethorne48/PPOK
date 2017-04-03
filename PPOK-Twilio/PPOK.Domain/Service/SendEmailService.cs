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
using PPOK.Domain.Types;

/// <summary>
/// This is a template to show how to create an email
/// </summary>
namespace PPOK.Domain.Service
{
    public class SendEmailService
    {
        //these will be the strings to hold the html for any emails that need to be sent
        private readonly EmailService emailService;

        public SendEmailService()
        {
            emailService = new EmailService(Config.BotEmail, Config.BotPassword); //it is not reading from config
        }

        public void Create(string toEmail, string messageBody,EventRefill t) 
        {
            emailService.SendEmail(
                from: Config.BotEmail,
                to: toEmail,
                subject: "Prescription Refill",
                body: Util.NamedFormat(messageBody, new { date = DateTime.Now, t }) //Must pass in Date from fill history separately
            );
        }
        public void Create(string toEmail, string messageBody, EventBirthday t)
        {
            emailService.SendEmail(
                from: Config.BotEmail,
                to: toEmail,
                subject: "Happy Birthday!!!",
                body: Util.NamedFormat(messageBody, new { date = DateTime.Now, t }) //Must pass in Date from fill history separately
            );
        }
        public void Create(string toEmail, string messageBody, EventRecall t)
        {
            emailService.SendEmail(
                from: Config.BotEmail,
                to: toEmail,
                subject: "Prescription Recall",
                body: Util.NamedFormat(messageBody, new { date = DateTime.Now, t }) //Must pass in Date from fill history separately
            );
        }
    }
}