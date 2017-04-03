using PPOK.Domain.Models;
using PPOK.Domain.Service;
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
            TwilioService.SendVoiceMessage(toPhoneNumber, startingRelativeUri);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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

        public ActionResult VoiceMessageGather(string messageBody)
        {
          
            TwilioGatherModel gatherer = new TwilioGatherModel()
            {
                MessageBody = messageBody,
                NoGatherMessage = "We didn't receive any input. Goodbye!",
                Options = GetGatherOptions(),
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
            List<TwilioGatherOption> optRedirects = GetGatherOptions();

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

        //consider persisting this in the database
        public List<TwilioGatherOption> GetGatherOptions()
        {
            var descriptions = new string[] { "Talk to a pharmacist", "Refill your prescription", "Unsubscribe from refill messages" };
            var handlerFuncs = new Func<string, string, object>[] {
                new Func<string,string, object>(GatherOpt1Test),
                new Func<string,string, object>(GatherOpt2Test),
                new Func<string, string, object>(GatherOpt3Test)
            };

            var opts = new List<TwilioGatherOption>();
            for (int i = 0; descriptions != null && i < descriptions.Length; i++)
            {
                TwilioGatherOption opt = new TwilioGatherOption()
                {
                    Digits = i.ToString(),
                    Description = descriptions[i],
                    Func = handlerFuncs[i],
                };
                opts.Add(opt);
            }

            return opts;
        }
}
}