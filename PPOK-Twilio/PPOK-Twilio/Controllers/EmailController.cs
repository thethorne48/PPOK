using PPOK.Domain.Service;
using PPOK.Domain.Types;
using PPOK.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPOK_Twilio.Controllers
{
    public class EmailController : Controller
    {
        public ActionResult HandleResponse(int optCode, int eventCode)
        {
            EventResponseModel erm = new EventResponseModel();
            object response = null;
            Event e;
            using (var service = new EventService())
            {
                e = service.Get(eventCode);
            }
            MessageResponseOption o;
            using (var service = new MessageResponseOptionService())
            {
                o = service.Get(optCode);
            }
            if (e != null && o != null)
            {
                response = EventProcessingService.HandleResponse(o, e);
            }
            if (response == null)
            {
                erm.ErrorMessage = "We're sorry, an application error has occurred. In order to process your request, please call your local pharmacy.";
            }
            else if (response is RedirectModel)
            {
                RedirectModel rm = (RedirectModel)response;
                return RedirectToAction(rm.Action, rm.Controller);
            }
            else if (response.GetType() == typeof(string))
            {
                erm.SuccessMessage = (string)response;
            }
            
            return View(erm);
        }
    }
}