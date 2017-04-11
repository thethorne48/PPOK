using PPOK.Domain.Models;
using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PPOK_Twilio.Controllers
{
    [Authorize(Roles = "Pharmacist")]
    public class ResendEventsController : BaseController
    {
        public ActionResult ResendEvents()
        {
            int id = User.Pharmacy.Code;
            //this id should be grabbed from the user to reflect current
            using (var service = new PrescriptionService())
            {
                var result = service.GetWhere(PrescriptionService.CodeCol);
                return View(new EventsModel(result);
            }
        }

        [HttpPost]
        public ActionResult resendevent(int Code)
        {
            using (var service = new ResendEventService())
            {
                service.ResendEvent(Code);
            }

            return RedirectToAction("ResendEvents", new RouteValueDictionary(
                        new { controller = "ResendEvent", action = "ResendEvents", Id = User.Pharmacy.Code }));
        }
    }
}