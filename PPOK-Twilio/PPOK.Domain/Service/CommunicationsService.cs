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
        public static bool Send(Event eventInfo, Patient p, bool overrideUnsubscribe)
        {
            bool isSent = true;
            string phone = p.Phone;
            string email = p.Email;

            switch (p.ContactPreference)
            {
                case ContactPreference.EMAIL:
                    new SendEmailService().Create(email, eventInfo.Message,"You've Got Mail"); //change the subject later
                    break;
                case ContactPreference.PHONE:
                    TwilioService.SendVoiceMessage(phone, "/Twilio/VoiceMessageGather?messageBody=" + eventInfo.Message);
                    break;
                case ContactPreference.TEXT:
                    TwilioService.SendSMSMessage(phone, eventInfo.Message);
                    break;
                case ContactPreference.NONE:
                default:
                    //send a message even if the contact preference is unsubscribed
                    if (overrideUnsubscribe)
                    {
                        TwilioService.SendVoiceMessage(phone, "/Twilio/VoiceMessageGather?messageBody=" + eventInfo.Message);
                    }
                    else
                    {
                        isSent = false;
                    }
                    break;
            }
            return isSent;
        }
    }
}
