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
        public static bool Send(Event eventInfo, Patient p, MessageTemplate template, bool overrideUnsubscribe)
        {
            bool isSent = true;
            string uniqueId;
            string phone = p.Phone;
            string email = p.Email;
            string message = eventInfo.Message;

            switch (p.ContactPreference)
            {
                case ContactPreference.EMAIL:
                    uniqueId = Email(email, message, template.Type);
                    break;
                case ContactPreference.PHONE:
                    uniqueId = Call(phone, message, template.Type);
                    break;
                case ContactPreference.TEXT:
                    uniqueId = Text(phone, message, template.Type);
                    break;
                case ContactPreference.NONE:
                default:
                    //send a message even if the contact preference is unsubscribed
                    if (overrideUnsubscribe)
                    {
                        uniqueId = Call(phone, message, template.Type);
                    }
                    else
                    {
                        isSent = false;
                    }
                    break;
            }
            return isSent;
        }

        private static string Call(string phone, string message, MessageTemplateType type)
        {
            var resource = TwilioService.SendVoiceMessage(phone, "/Twilio/VoiceMessage?toSay=" + message + "&templateType=" + type);
            return TwilioService.GetId(resource);
        }

        private static string Text(string phone, string message, MessageTemplateType type)
        {
            var resource = TwilioService.SendSMSMessage(phone, message);
            return TwilioService.GetId(resource);
        }

        private static string Email(string email, string message, MessageTemplateType type)
        {
            new SendEmailService().Create(email, message, "You've Got Mail"); //change the subject later
            return "123";
        }
    }
}
