using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPOK_Twilio.Controllers
{
    public class TwilioController : Controller
    {
        /// <summary>
        /// Note: This function name is used in Twilio configurations.
        /// Do NOT change the name without also updating the Twilio config for "Messaging>A message comes in"
        /// </summary>
        /// <returns></returns>
        public ActionResult ReceiveMessage()
        {
            string fromNumber = Request.Params["From"];
            if (!String.IsNullOrEmpty(fromNumber))
            {
                fromNumber = fromNumber.Substring(1);//phone numbers are prefixed with a +
            }
            string fromBody = Request.Params["Body"];
            string messageSid = Request.Params["MessageSid"];

            //string responseMessage = handleReceivedMessage(fromNumber, fromBody, messageSid);
            string responseMessage = "Specific response messages not yet implemented.";

            return View(model: responseMessage);
        }

        public ActionResult SendMessageExample(string toPhoneNumber, string messageBody)
        {
            var message = PPOK.Domain.Service.TwilioService.SendSMSMessage(toPhoneNumber, messageBody);
            return View();
        }
        
    }
}