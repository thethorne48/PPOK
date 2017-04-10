using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    public class EventProcessingService
    {
        public static void SendEvent(EventBirthday e)
        {
            if (e.Event.Status == EventStatus.InActive)
            {
                throw new ArgumentException("Cannot send an inactive event");
            }

            MessageTemplate template = GetTemplate(e);
            if (template == null)
            {
                throw new ArgumentNullException("Could not find a message template for the given media and type");
            }
            MergeToTemplate(e.Event, template, new { Patient = e.Patient });
            
            bool sent = CommunicationsService.Send(e.Event, e.Patient, false);
            if (sent)
            {
                UpdateEventStatus(e.Event);
                using (var service = new EventService())
                {
                    service.Update(e.Event);
                }
            }
        }

        public static void SendEvent(EventRecall e)
        {
            MessageTemplate template = GetTemplate(e);
            if (template == null)
            {
                throw new ArgumentNullException("Could not find a message template for the given media and type");
            }
            MergeToTemplate(e.Event, template, new { Patient = e.Patient, Drug = e.Drug });

            bool sent = CommunicationsService.Send(e.Event, e.Patient, true);
            if (sent)
            {
                UpdateEventStatus(e.Event);
                using (var service = new EventService())
                {
                    service.Update(e.Event);
                }
            }
        }

        public static void SendEvent(EventRefill e)
        {
            MessageTemplate template = GetTemplate(e);
            if (template == null)
            {
                throw new ArgumentNullException("Could not find a message template for the given media and type");
            }
            MergeToTemplate(e.Event, template, new { Prescription = e.Prescription });

            bool sent = CommunicationsService.Send(e.Event, e.Prescription.Patient, false);
            if (sent)
            {
                UpdateEventStatus(e.Event);
                using (var service = new EventService())
                {
                    service.Update(e.Event);
                }
            }
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

        private static void MergeToTemplate(Event e, MessageTemplate t, object templateParams)
        {
            if (t != null)
            {
                e.Message = Utility.Util.NamedFormat(t.Content, templateParams);
            }
        }

        private static MessageTemplate GetTemplate(Event e)
        {
           foreach (var refill in e.Refills)
            {
                return GetTemplate(refill);
            }
            foreach (var recall in e.Recalls)
            {
                return GetTemplate(recall);
            }
            foreach (var birthday in e.Birthdays)
            {
                return GetTemplate(birthday);
            }
        }

        private static MessageTemplate GetTemplate(EventBirthday e)
        {
            return GetMessageTemplate(MessageTemplateType.HAPPYBIRTHDAY, GetMedia(e.Patient.ContactPreference));
        }

        private static MessageTemplate GetTemplate(EventRecall e)
        {
            return GetMessageTemplate(MessageTemplateType.RECALL, GetMedia(e.Patient.ContactPreference));
        }

        private static MessageTemplate GetTemplate(EventRefill e)
        {
            MessageTemplateType type;
            switch (e.Event.Status)
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
                    return null;
            }
            return GetMessageTemplate(type, GetMedia(e.Prescription.Patient.ContactPreference));
        }
    }
}
