﻿using PPOK.Domain.Types;
using PPOK.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using static PPOK.Domain.Utility.Config;
using System.Linq;

namespace PPOK.Domain.Service
{
    /// <summary>
    /// Services API requests and responses with Twilio. Requires a localtunnel to be open with a base url
    /// equal to the ExternalUrl attribute for this class. For a trial account, your phone number 
    /// must be verified in Twilio to communicate with this API.
    /// </summary>
    public class TwilioService
    {
        //https://support.twilio.com/hc/en-us/articles/223134027-Twilio-support-for-STOP-BLOCK-and-CANCEL-SMS-STOP-filtering
        private static List<string> twilioUnsubscribeVerbs = new List<string>()
        {
            "STOP", "STOPALL", "UNSUBSCRIBE", "CANCEL", "END", "QUIT"
        };

        private static List<string> twilioSubscribeVerbs = new List<string>()
        {
            "START", "YES", "UNSTOP"
        };

        public static string GetId(CallResource call)
        {
            return call.Sid;
        }

        public static string GetId(MessageResource text)
        {
            return text.Sid;
        }

        private static Patient GetPatientFromPhone(string phoneNumber)
        {
            Patient p;
            using (var service = new PatientService())
            {
                p = service.GetWhere(PatientService.PhoneCol == phoneNumber).FirstOrDefault();
            }
            return p;
        }

        public static void HandleSMSResponse(string fromNumber, string fromBody, string messageSid)
        {
            string responseString = "";

            //unsubscribe
            if (twilioUnsubscribeVerbs.Find(v => v.ToLower().Equals(fromBody)) != null) {
                Patient p = GetPatientFromPhone(fromNumber);
                if (p != null)
                {
                    responseString = EventProcessingService.UnsubscribePatient(p);
                }
            }

            //subscribe
            if (twilioSubscribeVerbs.Find(v => v.ToLower().Equals(fromBody)) != null)
            {
                Patient p = GetPatientFromPhone(fromNumber);
                if (p != null && p.ContactPreference == ContactPreference.NONE)
                {
                    responseString = EventProcessingService.Subscribe(p);
                }
            }
            
            //system-specific responses
            if (responseString.Length == 0)
            {
                List<Event> openEvents = GetOpenSMSEventsFor(fromNumber);

                foreach (Event e in openEvents)
                {
                    MessageTemplateType templateType = EventProcessingService.GetTemplateType(e);
                    List<MessageResponseOption> opts = CommunicationsService.GetResponseOptions(templateType);
                    MessageResponseOption opt = opts.Find(o => { return o.Verb.ToLower().Equals(fromBody); });

                    var response = EventProcessingService.HandleResponse(opt, e);
                    if (response.GetType() == typeof(string))
                    {
                        responseString = (string)response;
                    }
                }
            }
            
            if (responseString.Length > 0)
            {
                SendSMSMessage(fromNumber, responseString);
            } else
            {
                throw new ArgumentException("Unexpected response: " + fromBody + " from message: " + messageSid);
            }
        }

        private static List<Event> GetOpenSMSEventsFor(string fromNumber)
        {
            List<Event> events;
            //add filter with one week ago when Jon adds date comparisons to CRUDService
            DateTime oneWeekAgo = DateTime.Today.AddDays(-7);
            using (var eventService = new EventService())
            using (var patientService = new PatientService())
            using (var service = new EventHistoryService())
            {
                events = service.GetWhere(
                    (PatientService.PhoneCol.In("Event") == fromNumber) &
                    (PatientService.ContactPreferenceCol.In("Event") == ContactPreference.TEXT &
                    EventService.StatusCol == EventStatus.Sent &
                    EventHistoryService.DateCol >= oneWeekAgo)).Select(hist => hist.Event).ToList();
            }
            return events;
        }

        /// <summary>
        /// Send a SMS message to the specified phone number.
        /// You can specify instructions on how to respond separately from the main message body by including message options.
        /// </summary>
        /// <param name="toNumber">phone number to send the SMS to. Standard rates apply.</param>
        /// <param name="messageBody">contents of the SMS message</param>
        /// <param name="options">how the user is expected to reply</param>
        /// <returns></returns>
        public static MessageResource SendSMSMessage(string toNumber, string messageBody, List<MessageResponseOption> options = null)
        {
            //the paid account is not set up to handle sending text messages; there are no phone resources we can grab
            TwilioClient.Init(TwilioTrialAccountSid, TwilioTrialAuthToken);

            string optionsStr = (options == null) ? "" : GetTextOptions(options);
            var phoneResource = GetTextPhoneResource();

            var message = MessageResource.Create(
                 from: phoneResource.PhoneNumber,
                 to: new PhoneNumber(toNumber),
                 body: messageBody + optionsStr);

            if (message.ErrorCode != null)
            {
                throw new Exception("Twilio encountered an error: " + message.ErrorCode + " - " + message.ErrorMessage);
            }

            return message;
        }

        private static IncomingPhoneNumberResource GetTextPhoneResource()
        {
            return IncomingPhoneNumberResource.Read().FirstOrDefault();
        }

        private static OutgoingCallerIdResource GetVoicePhoneResource()
        {
            return OutgoingCallerIdResource.Read().FirstOrDefault();
        }

        /// <summary>
        /// Calls the specified phone number with the starting TwiML message script found at the relative url. 
        /// Note that the relative url cannot contain characters that are url-encoded like spaces (Twilio Api throws an invalid url exception, even if the url is valid.
        /// Ideally query parameters in the relativeUrl should be simple, like a record's unique id.
        /// </summary>
        /// <param name="toNumber">phone number to open a phone call to. Standard rates apply.</param>
        /// <param name="relativeUrl">Relative url to the page that generates the TwiML for this message's contents, i.e. "MyController/TwilioCall?param=3".</param>
        public static CallResource SendVoiceMessage(string toNumber, string relativeUrl)
        {
            TwilioClient.Init(TwilioAccountSid, TwilioAuthToken);

            var phoneResource = GetVoicePhoneResource();
            CallResource call = CallResource.Create(to: new PhoneNumber(toNumber),
                                           from: phoneResource.PhoneNumber,
                                           url: new Uri(ExternalUrl + relativeUrl));
            return call;
        }

        private static string GetTextOptions(List<MessageResponseOption> options)
        {
            options = options.FindAll(o => { return !(string.IsNullOrWhiteSpace(o.Verb) || string.IsNullOrWhiteSpace(o.ShortDescription)); });
            if (options.Count == 0)
            {
                return "";
            }
            
            StringBuilder sb = new StringBuilder(" Reply with ");
                
            for (int i = 0; i < options.Count; i++)
            {
                MessageResponseOption opt = options[i];
                sb.Append(opt.Verb.ToUpper());
                sb.Append(" to ");
                sb.Append(opt.ShortDescription);
                if (i + 1 < options.Count)
                {
                    sb.Append(", ");
                }
                else
                {
                    sb.Append(".");
                }
            }
            return sb.ToString();
        }

        public static List<TwilioGatherOption> GetGatherOptions(MessageTemplateType type)
        {
            List<MessageResponseOption> respOpts = CommunicationsService.GetResponseOptions(type);
            respOpts = respOpts.FindAll(o => { return !string.IsNullOrWhiteSpace(o.LongDescription); });
            
            //Func<string, string, object>[] handlerFuncs = null;
            
            var opts = new List<TwilioGatherOption>();
            for (int i = 0; respOpts != null && i < respOpts.Count; i++)
            {
                TwilioGatherOption opt = new TwilioGatherOption()
                {
                    Digits = i.ToString(),
                    Description = respOpts[i].LongDescription,
                    ResponseOption = respOpts[i]
                };
                opts.Add(opt);
            }

            return opts;
        }
    }
}

