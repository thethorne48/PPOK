using PPOK.Domain.Models;
using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPOK_Twilio.Controllers
{
    //[Authorize(Roles ="Admin")] //System is just System
    public class SearchController : BaseController
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSingleEvent(int id)
        {
            using (var service = new EventRefillService())
            {
                var result = service.Get(id);
                return Json(new SearchDetailsModal(result));
            }
        }

        [HttpPost]
        public JsonResult Inactivate(int id)
        {
            Pharmacist pharm = new Pharmacist();
            string eventType = null;
            using (var pharService = new PharmacistService())
            {
                //pharm = pharService.Get(User.Code);
                pharm = pharService.Get(1);

            }
            using (var eservice = new EventService())
            {
                var temp = eservice.Get(id);
                if (temp.Refills.FirstOrDefault() != null)
                {
                    eventType = "Refill Event";
                }
                else if (temp.Birthdays.FirstOrDefault() != null)
                {
                    eventType = "Birthday Event";
                }
                else
                    eventType = "Recall Event";
            }
            if (eventType == "Refill Event")
            {
                using (var service = new EventRefillService())
                {
                    var Er = service.Get(id);
                    using (var historyService = new EventHistoryService())
                    {
                        EventHistory Eh = new EventHistory(Er.Event, EventStatus.InActive, DateTime.Now);
                        historyService.Create(Eh);
                    }
                    using (var eventService = new EventService())
                    {
                        var up = eventService.Get(Er.Event.Code);
                        up.Status = EventStatus.InActive;
                        eventService.Update(up);
                    }
                }
            }
            else if (eventType == "Recall Event")
            {
                using (var service = new EventRecallService())
                {
                    var Er = service.Get(id);
                    using (var historyService = new EventHistoryService())
                    {
                        EventHistory Eh = new EventHistory(Er.Event, EventStatus.InActive, DateTime.Now);
                        historyService.Create(Eh);
                    }
                    using (var eventService = new EventService())
                    {
                        var up = eventService.Get(Er.Event.Code);
                        up.Status = EventStatus.InActive;
                        eventService.Update(up);
                    }
                }
            }
            else if (eventType == "Birthday Event")
            {
                using (var service = new EventBirthdayService())
                {
                    var Er = service.Get(id);
                    using (var historyService = new EventHistoryService())
                    {
                        EventHistory Eh = new EventHistory(Er.Event, EventStatus.InActive, DateTime.Now);
                        historyService.Create(Eh);
                    }
                    using (var eventService = new EventService())
                    {
                        var up = eventService.Get(Er.Event.Code);
                        up.Status = EventStatus.InActive;
                        eventService.Update(up);
                    }
                }
            }
            return Json(true);

        }

        [HttpPost]
        public JsonResult GetAllEvents()
        {
            List<SearchModel> result = new List<SearchModel>();
            //using (var service = new EventService())
            //{

            //    var temp = service.GetAll();

            //my assumption is that for each event there is only one birthday, recall, or refill
            //and the event history will be full of events that i can display under details
            //   foreach (var l in temp)
            //    {
            //        if(l.Birthdays.FirstOrDefault()==null && l.Recalls.FirstOrDefault() == null) //breaks here
            //        {
            //            //this is a refill event
            //            result.Add(new SearchModel(l.Refills.FirstOrDefault()));
            //        }
            //        else if (l.Refills.FirstOrDefault() == null && l.Recalls.FirstOrDefault() == null)
            //        {
            //            //this is a birthday event
            //            result.Add(new SearchModel(l.Birthdays.FirstOrDefault()));
            //        }
            //    }
            //}
            using (
                var service = new EventRefillService())
            {
                var test = service.GetAll();
                //make a model to hold this
                foreach (var t in test)
                {
                    result.Add(new SearchModel(t));
                }
            }
            using (var service = new EventRecallService())
            {
                var test = service.GetAll();
                //make a model to hold this
                foreach (var t in test)
                {
                    result.Add(new SearchModel(t));
                }
            }
            using (var service = new EventBirthdayService())
            {
                var test = service.GetAll();
                //make a model to hold this
                foreach (var t in test)
                {
                    result.Add(new SearchModel(t));
                }
            }
            return Json(result);
        }
    }
}