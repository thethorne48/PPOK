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
        public static void Send(EventBirthday e)
        {
            MergeToTemplate(e);
            bool sent = Send(e.Event, e.Patient, false);
            if (sent)
            {
                using (var service = new EventBirthdayService())
                {
                    service.Update(e);
                }
            }
        }

        public static void Send(EventRecall e)
        {
            MergeToTemplate(e);
            bool sent = Send(e.Event, e.Patient, true);
            if (sent)
            {
                using (var service = new EventRecallService())
                {
                    service.Update(e);
                }
            }
        }

        public static void Send(EventRefill e)
        {
            bool sent = Send(e.Event, e.Prescription.Patient, false);
            if (sent)
            {
                using (var service = new EventRefillService())
                {
                    service.Update(e);
                }
            }
        }

        private static bool Send(Event eventInfo, Patient p, bool overrideUnsubscribe)
        {
            bool isSent = true;
            string phone = p.Phone;
            string email = p.Email;
            
            switch (p.ContactPreference)
            {
                case ContactPreference.EMAIL://please change this horrid horrid code
                    try {
                        new SendEmailService().Create(email, eventInfo.Message, eventInfo.Birthdays.FirstOrDefault());
                    }
                    catch
                    {
                        new SendEmailService().Create(email, eventInfo.Message, eventInfo.Refills.FirstOrDefault());

                    }
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
                    } else
                    {
                        isSent = false;
                    }
                    break;
            }

            if (isSent)
            {
                //update the event to "Sent"
                using (var service = new EventHistoryService())
                {
                    EventHistory eh = new EventHistory();
                    eh.Date = DateTime.Now;
                    eh.Status = EventStatus.Sent;
                    eh.Event = eventInfo;
                    service.Create(eh);
                }
            }
            return isSent;
        }

        private static MessageTemplate GetMessageTemplate(MessageTemplateType type, MessageTemplateMedia media)
        {
            MessageTemplate t;
            using (var service = new MessageTemplateService())
            {
                t = service.GetWhere(MessageTemplateService.TypeCol == type & MessageTemplateService.MediaCol == media).FirstOrDefault();
            }
            return t;
        }

        private static MessageTemplateMedia GetMedia(ContactPreference cp)
        {
            switch(cp)
            {
                case ContactPreference.EMAIL:
                    return MessageTemplateMedia.EMAIL;
                case ContactPreference.PHONE:
                case ContactPreference.TEXT:
                    return MessageTemplateMedia.PHONE;
                case ContactPreference.NONE:
                default:
                    return MessageTemplateMedia.NONE;
            }
        }

        private static void MergeToTemplate(EventBirthday e)
        {
            MessageTemplate t = GetMessageTemplate(MessageTemplateType.HAPPYBIRTHDAY, GetMedia(e.Patient.ContactPreference));
            e.Event.Message = Utility.Util.NamedFormat(t.Content, e.Patient);
        }

        private static void MergeToTemplate(EventRecall e)
        {
            MessageTemplate t = GetMessageTemplate(MessageTemplateType.RECALL, GetMedia(e.Patient.ContactPreference));
            e.Event.Message = Utility.Util.NamedFormat(t.Content, new { Patient = e.Patient, Drug = e.Drug });
        }

        private static void MergeToTemplate(EventRefill e)
        {
            MessageTemplate t = GetMessageTemplate(MessageTemplateType.REFILL, GetMedia(e.Prescription.Patient.ContactPreference));
            e.Event.Message = Utility.Util.NamedFormat(t.Content, e.Prescription);
        }
    }
}
