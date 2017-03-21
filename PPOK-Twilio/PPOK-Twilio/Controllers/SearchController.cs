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
        public JsonResult GetAllEvents()
        {
            using (var service = new EventService())
            {
                var test = service.GetAll();
                //make a model to hold this
                return Json(test);
            }
        }
    }
}