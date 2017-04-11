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
        //don't do event type detection via subclass, pass an event and do it via Event.Type
        public static void Send(Event e)
        {
            if (e.Status == EventStatus.InActive)
            {
                throw new ArgumentException("Cannot send an inactive event");
            }
            bool merged = MergeToTemplate(e);
            if (!merged)
            {
                throw new ArgumentNullException("Could not find a message template for the given media and type");
            }
            bool sent = Send(e, e.Patient, false);
            if (sent)
            {
                using (var service = new EventService())
                {
                    service.Update(e);
                }
            }
        }

        public static void Send(EventRecall e)
        {
            MergeToTemplate(e);
            bool sent = Send(e.Event, e.Event.Patient, true);
            if (sent)
            {
                using (var service = new EventService())
                {
                    service.Update(e.Event);
                }
            }
        }

        public static void Send(EventRefill e)
        {
            MergeToTemplate(e);
            bool sent = Send(e.Event, e.Prescription.Patient, false);
            if (sent)
            {
                using (var service = new EventService())
                {
                    service.Update(e.Event);
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

            if (isSent)
            {
                UpdateEventStatus(eventInfo);
            }
            return isSent;
        }

        private static void UpdateEventStatus(Event eventInfo)
        {
            EventStatus newStatus;
            switch (eventInfo.Status)
            {
                case EventStatus.ToSend:
                case EventStatus.Sent:
                    newStatus = EventStatus.Sent;
                    break;
                case EventStatus.Fill:
                    newStatus = EventStatus.Complete;
                    break;
                case EventStatus.InActive:
                default:
                    newStatus = eventInfo.Status;
                    break;
            }
            //update the event to the new status
            //currently we have to update the Event object as well as create a history entry for it
            eventInfo.Status = newStatus;
            using (var service = new EventHistoryService())
            {
                EventHistory eh = new EventHistory();
                eh.Date = DateTime.Now;
                eh.Status = newStatus;
                eh.Event = eventInfo;
                service.Create(eh);
            }
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
            switch (cp)
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

        private static bool MergeToTemplate(Event e)
        {
            MessageTemplate t = GetMessageTemplate(MessageTemplateType.HAPPYBIRTHDAY, GetMedia(e.Patient.ContactPreference));
            if (t != null)
            {
                e.Message = Utility.Util.NamedFormat(t.Content, new { e.Patient });
                return true;
            }
            return false;
        }

        private static bool MergeToTemplate(EventRecall e)
        {
            MessageTemplate t = GetMessageTemplate(MessageTemplateType.RECALL, GetMedia(e.Event.Patient.ContactPreference));
            if (t != null)
            {
                e.Event.Message = Utility.Util.NamedFormat(t.Content, new { Patient = e.Event.Patient, Drug = e.Drug });
                return true;
            }
            return false;
        }

        private static bool MergeToTemplate(EventRefill e)
        {
            MessageTemplateType type;
            switch(e.Event.Status)
            {
                case EventStatus.ToSend:
                case EventStatus.Sent:
                    type = MessageTemplateType.REFILL;
                    break;
                case EventStatus.Fill:
                    type = MessageTemplateType.REFILL_RESPONSE;
                    break;
                case EventStatus.Complete:
                    type = MessageTemplateType.REFILL_PICKUP;
                    break;
                case EventStatus.InActive:
                default:
                    return false;
            }
            MessageTemplate t = GetMessageTemplate(type, GetMedia(e.Prescription.Patient.ContactPreference));
            if (t != null)
            {
                e.Event.Message = Utility.Util.NamedFormat(t.Content, e.Prescription);
                return true;
            }
            return false;
        }
    }
}
