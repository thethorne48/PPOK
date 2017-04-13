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
		public static void SendEvents(List<Event> events, int pharmacyId)
		{
			List<MessageTemplate> templates = GetTemplatesForPharmacy(pharmacyId);
			foreach(Event e in events)
			{
				MergeAndSend(e, templates);
			}
		}
		public static void SendEvent(Event e, int pharmacyId)
		{
			List<MessageTemplate> templates = GetTemplatesForPharmacy(pharmacyId);
			MergeAndSend(e, templates);
			
		}

		public static void SendEvents(List<Event> events, MessageTemplate template)
		{
			foreach (Event e in events)
			{
				MergeAndSend(e, template);
			}
		}
		public static void SendEvent(Event e, MessageTemplate template)
		{
			MergeAndSend(e, template);

		}

		private static void MergeAndSend(Event e, MessageTemplate template)
		{
			if (e.Status == EventStatus.InActive)
			{
				throw new ArgumentException("Cannot send an inactive event");
			}
			
			object templateObject = GetTemplateObject(e);
			

			if (template == null)
			{
				throw new ArgumentNullException("Could not find a message template for the given media and type");
			}

			MergeToTemplate(e, template, templateObject);

			bool sent = CommunicationsService.Send(e, template, false);
			if (sent)
			{
				UpdateEventStatus(e);
				using (var service = new EventService())
				{
					service.Update(e);
				}
			}
		}

		private static void MergeAndSend(Event e, List<MessageTemplate> pharmacyTemplates)
		{
			if (e.Status == EventStatus.InActive)
			{
				throw new ArgumentException("Cannot send an inactive event");
			}
			
			MessageTemplateType templateType = GetTemplateType(e);
			MessageTemplateMedia templateMedia = GetMedia(e.Patient.ContactPreference);
			object templateObject = GetTemplateObject(e);

			MessageTemplate template = GetMessageTemplate(pharmacyTemplates, templateType, templateMedia);

			if (template == null)
			{
				throw new ArgumentNullException("Could not find a message template for the given media and type");
			}

			MergeToTemplate(e, template, templateObject);

			bool sent = CommunicationsService.Send(e, template, false);
			if (sent)
			{
				UpdateEventStatus(e);
				using (var service = new EventService())
				{
					service.Update(e);
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

		private static MessageTemplate GetMessageTemplate(List<MessageTemplate> options, MessageTemplateType type, MessageTemplateMedia media)
		{
			MessageTemplate template;
			template = options.Find((t) => { return (t.Type == type && t.Media == media); });
			return template;
		}

		private static void MergeToTemplate(Event e, MessageTemplate t, object templateParams)
		{
			if (t != null)
			{
				e.Message = Utility.Util.NamedFormat(t.Content, templateParams);
			}
		}
		
		private static List<MessageTemplate> GetTemplatesForPharmacy(int pId)
		{
			List<MessageTemplate> templates;
			using (var pharmService = new PharmacyService())
			using (var service = new MessageTemplateService())
			{
				templates = service.GetWhere(PharmacyService.CodeCol == pId);
			}
			return templates;
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

		private static MessageTemplateType GetTemplateType(Event e)
		{
			MessageTemplateType templateType;
			switch (e.Type)
			{
				case EventType.BIRTHDAY:
					templateType = MessageTemplateType.HAPPYBIRTHDAY;
					break;
				case EventType.RECALL:
					templateType = MessageTemplateType.RECALL;
					break;
				case EventType.REFILL:
					templateType = GetRecallTemplateType(e);
					break;
				default:
					throw new Exception($"Unknown EventType {e.Type}");
			}
			return templateType;
		}

		private static MessageTemplateType GetRecallTemplateType(Event e)
		{
			MessageTemplateType type;
			switch (e.Status)
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
					throw new Exception($"No MessageTemplateType for ({e.Type}, {e.Status})");
				default:
					throw new Exception($"Unknown EventStatus {e.Status}");
			}
			return type;
		}

		private static object GetTemplateObject(Event e)
		{
			object templateObject;
			switch (e.Type)
			{
				case EventType.BIRTHDAY:
					templateObject = new { Patient = e.Patient };
					break;
				case EventType.RECALL:
					templateObject = new { Patient = e.Patient, Drug = e.Recalls.FirstOrDefault().Drug };
					break;
				case EventType.REFILL:
					templateObject = new { Prescription = e.Refills.FirstOrDefault().Prescription };
					break;
				default:
					templateObject = new { };
					break;
			}
			return templateObject;
		}
	}
}
