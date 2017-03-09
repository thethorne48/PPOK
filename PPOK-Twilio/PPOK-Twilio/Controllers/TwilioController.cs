using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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

        public ActionResult SendSMSExample(string toPhoneNumber, string messageBody)
        {
            var message = PPOK.Domain.Service.TwilioService.SendSMSMessage(toPhoneNumber, messageBody);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult SendVoiceMessageExample(string toPhoneNumber)
        {
            PPOK.Domain.Service.TwilioService.SendVoiceMessage(toPhoneNumber, "Twilio/VoiceMessageExampleStart");
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult VoiceMessageExampleStart()
        {
            //This must be a partial view, otherwise MVC will prepend a blank line, which is invalid for XML
            return PartialView();
        }
        
    }
}