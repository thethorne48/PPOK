using PPOK.Domain.Models;
using PPOK.Domain.Service;
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
            using (var service = new EventRefillService())
            {
                //see refill event flow, this will probably be similar except no fill history
                return Json(true);
            }
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