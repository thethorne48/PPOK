using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPOK_Twilio.Controllers
{
    public class TwilioController : Controller
    {
        
        public ActionResult SendMessageExample(string toPhoneNumber, string messageBody)
        {
            var message = PPOK.Domain.Service.TwilioService.SendSMSMessage(toPhoneNumber, messageBody);
            return View();
        }
        
    }
}