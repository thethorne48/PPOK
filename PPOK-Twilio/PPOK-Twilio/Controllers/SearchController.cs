using PPOK.Domain.Models;
using PPOK.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPOK_Twilio.Controllers
{
    public class SearchController : BaseController
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSingleEvent(int id) //this works, but the redirect to action wont refresh the page
        {
            using (var service = new EventService())
            {
                var result = service.Get(id);
                return Json(result);
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