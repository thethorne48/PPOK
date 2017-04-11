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
            List<Prescription> list = new List<Prescription>();
            using (var service = new EventService())
            {
                var result = service.GetWhere(EventService.StatusCol == EventStatus.Sent & EventService.TypeCol == EventType.REFILL);
                foreach (Event e in result)
                {

                }
                return View(new EventsModel(list);
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