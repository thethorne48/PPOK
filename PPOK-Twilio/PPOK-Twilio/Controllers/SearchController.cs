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
            using (var service = new EventService())
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
                //need to just inactivate
                //service.Delete(id);
                //grab record of that id
                //change status to inactive

                return Json(true);
            }
        }

        [HttpPost]
        public JsonResult GetAllEvents()
        {
            using (var service = new EventService())
            {
                List<SearchModel> result = new List<SearchModel>();
                var test = service.GetAll();
                //make a model to hold this
                foreach (var t in test)
                {
                    result.Add(new SearchModel(t));
                }
                return Json(result);
            }
        }
    }
}