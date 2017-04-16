﻿using PPOK.Domain.Models;
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
            List<Prescription> list = new List<Prescription>();
            //this id should be grabbed from the user to reflect current
            using (var service = new EventService())
            {
                var result = service.GetWhere(EventService.StatusCol == EventStatus.Sent & EventService.TypeCol == EventType.REFILL);
                foreach(Event e in result)
                {
                        list.Add(e.Refills.FirstOrDefault().Prescription);
                }
                return View(new EventsModel(list));
            }
        }

        [HttpPost]
        public JsonResult GetSingleEvent(int Code)
        {
            return Json(new EventModel(Code));
        }

        [HttpPost]
        public ActionResult resendevents(int Code)
        {
            using (var service = new ResendEventService())
            {
                service.ResendEvent(Code);
            }

            return RedirectToAction("ResendEvents", new RouteValueDictionary(
                        new { controller = "ResendEvents", action = "ResendEvents", Id = User.Pharmacy.Code }));
        }
    }
}