using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public static class CommunicationsService
    {
        public static string Send(Event eventInfo, MessageTemplate template, bool overrideUnsubscribe)
        {
            Patient p = eventInfo.Patient;
            string phone = p.Phone;
            string message = eventInfo.Message;
            string uniqueId = null;

            switch (p.ContactPreference)
            {
                case ContactPreference.EMAIL:
                    uniqueId = Email(eventInfo, GetResponseOptions(template.Type));
                    break;
                case ContactPreference.PHONE:
                    uniqueId = Call(phone, eventInfo);
                    break;
                case ContactPreference.TEXT:
                    uniqueId = Text(phone, message, GetResponseOptions(template.Type));
                    break;
                case ContactPreference.NONE:
                default:
                    //send a message even if the contact preference is unsubscribed
                    if (overrideUnsubscribe)
                    {
                        uniqueId = Call(phone, eventInfo);
                    }
                    break;
            }
            return uniqueId;
        }

        private static string Call(string phone, Event e)
        {
            var resource = TwilioService.SendVoiceMessage(phone, "Twilio/VoiceMessage?eventCode=" + e.Code);

            return TwilioService.GetId(resource);
        }

        private static string Text(string phone, string message, List<MessageResponseOption> options)
        {
            var resource = TwilioService.SendSMSMessage(phone, message, options);
            return TwilioService.GetId(resource);
        }

        private static string Email(Event e, List<MessageResponseOption> options)
        {
            using (var service = new EmailService())
            {
                string htmlBody = EmailTemplateService.GetHTML(e, options);
                service.SendEmail(e.Patient.Email, "You've Got Mail from " + e.Patient.Pharmacy.Name, htmlBody);
            }
            return "123";
        }

        public static List<MessageResponseOption> GetResponseOptions(MessageTemplateType templateType)
        {
            List<MessageResponseOption> opts;
            using (var service = new MessageResponseOptionService())
            {
                opts = service.GetWhere(MessageResponseOptionService.MessageTemplateTypeCol == templateType);
            }
            return opts;
        }
    }
}
