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
    [Authorize(Roles ="Admin")]
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
            using (var service = new EventService())
            {
                //get the event
                var even = service.Get(id);
                //create the history entry
                using (var historyService = new EventHistoryService())
                {
                    EventHistory history = new EventHistory(even, EventStatus.InActive, DateTime.Now);
                    historyService.Create(history);
                }
                //update status and database
                even.Status = EventStatus.InActive;
                service.Update(even);
            }
            return Json(true);
        }

        [HttpPost]
        public JsonResult GetAllEvents()
        {
            List<SearchModel> models = new List<SearchModel>();
            using(var service = new EventService())
            {
                models = service.GetAll()
                    .Select(e => new SearchModel(e))
                    .ToList();
            }
            return Json(models);
        }
    }
}