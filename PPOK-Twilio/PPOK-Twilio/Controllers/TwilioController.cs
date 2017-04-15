using PPOK.Domain.Models;
using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace PPOK_Twilio.Controllers
{
    public class TwilioController : Controller
    {
        /// <summary>
        /// Note: This function name is used in Twilio configurations.
        /// Do NOT change the name without also updating the Twilio config for the PPOk Message Service
        /// </summary>
        /// <returns></returns>
        public ActionResult ReceiveSMS()
        {
            string fromNumber = Request.Params["From"];
            if (!String.IsNullOrEmpty(fromNumber))
            {
                fromNumber = fromNumber.Substring(1);//phone numbers are prefixed with a +
            }
            string fromBody = Request.Params["Body"];
            string messageSid = Request.Params["MessageSid"];

            //Note: need to support unsubscribing / subscribing key words that Twilio uses
            //https://support.twilio.com/hc/en-us/articles/223134027-Twilio-support-for-STOP-BLOCK-and-CANCEL-SMS-STOP-filtering-
            //EventService.handleReceivedMessage(fromNumber, fromBody, messageSid);

            return View();
        }

        public ActionResult SendVoiceMessage(string toPhoneNumber, string startingRelativeUri)
        {
            var call = TwilioService.SendVoiceMessage(toPhoneNumber, startingRelativeUri);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult VoiceMessage(int eventCode, string toDial)
        {
            Event e;
            using(var service = new EventService())
            {
                e = service.Get(eventCode);
            }
            if (e == null)
            {
                throw new ArgumentException("Invalid event code: " + eventCode);
            }

            if (!String.IsNullOrWhiteSpace(toDial))
            {
                return VoiceMessageDial("", toDial);
            }
            MessageTemplateType templateType = EventProcessingService.GetTemplateType(e);
            List<TwilioGatherOption> options = TwilioService.GetGatherOptions(templateType);
            if (options.Count > 0)
            {
                return VoiceMessageGather(e.Message, options);
            }
            return VoiceMessageSay(null, e.Message);
        }

        public ActionResult VoiceMessageSay(string redirectRelativeUri, string toSay)
        {
            TwilioSayModel say = new TwilioSayModel()
            {
                MessageBody = toSay,
                Redirect = redirectRelativeUri
            };
            //This must be a partial view, otherwise MVC will prepend a blank line, which is invalid for XML
            return PartialView("VoiceMessageSay", say);
        }

        public ActionResult VoiceMessageDial(string toSay, string toDial)
        {
            TwilioDialModel dial = new TwilioDialModel()
            {
                MessageBody = toSay,
                DialTo = new Twilio.Types.PhoneNumber(toDial)
            };
            //This must be a partial view, otherwise MVC will prepend a blank line, which is invalid for XML
            return PartialView("VoiceMessageDial", dial);
        }

        public ActionResult VoiceMessageGather(string messageBody, List<TwilioGatherOption> gatherOptions)
        {
          
            TwilioGatherModel gatherer = new TwilioGatherModel()
            {
                MessageBody = messageBody,
                NoGatherMessage = "We didn't receive any input. Goodbye!",
                Options = gatherOptions,
                Redirect = "/Twilio/VoiceMessageGathered"
            };
            
            //This must be a partial view, otherwise MVC will prepend a blank line, which is invalid for XML
            return PartialView("VoiceMessageGather", gatherer);
        }

        [HttpPost]
        public ActionResult VoiceMessageGathered(string Digits)
        {
            string fromNumber = Request.Params["From"];
            string callSid = Request.Params["CallSid"];
            /*
            Event e;
            using (var service = new EventService())
            {
                //e = service.GetWhere(EventService.ExternalIdCol == callSid);
            }

            MessageTemplate template = EventProcessingService.GetTemplate(e);
            using (var eventService = new EventService())
            using (var service = new MessageTemplateService())
            {
                template = service.GetWhere(MessageTemplateService.TypeCol )
            }*/
            List<TwilioGatherOption> optRedirects = GetGatherOptions(MessageTemplateType.REFILL);//template.Type);

            bool isActionFound = false;
            object response = null;
            foreach (var opt in optRedirects)
            {
                if (opt.Digits.Equals(Digits))
                {
                    response = opt.Func.Invoke(fromNumber, callSid);
                    
                    isActionFound = true;
                }
            }
            
            if (!isActionFound)
            {
                response = "Action not found";
            }

            if (response is ActionResult) {
                return (ActionResult) response;
            }
            
            return RedirectToAction("VoiceMessageSay", new { toSay = response.ToString() });
        }

        public ActionResult SendSMSExample(string toPhoneNumber, string messageBody)
        {
            var message = TwilioService.SendSMSMessage(toPhoneNumber, messageBody);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult SendVoiceMessageExample(string toPhoneNumber)
        {
            return RedirectToAction("SendVoiceMessage",
                new {
                    toPhoneNumber = toPhoneNumber,
                    startingRelativeUri = "Twilio/VoiceMessageSay?toSay=Thanks for trying our documentation. Enjoy!"
                });
        }

        public ActionResult GatherOpt1Test(string fromNumber, string callSid)
        {
            string pharmacyPhone = "14151234567";
            string pharmacyName = "your pharmacy";
            string toSay = "Now dialing " + pharmacyName;
            return VoiceMessageDial(toSay, pharmacyPhone);
        }

        public string GatherOpt2Test(string fromNumber, string callSid)
        {
            return "Stub for refill completed successfully";
        }

        public string GatherOpt3Test(string fromNumber, string callSid)
        {
            return "Stub for unsubscribe completed successfully";
        }
    }
}