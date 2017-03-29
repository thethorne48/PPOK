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
    public class SystemAdminController : Controller
    {
        // GET: SystemAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pharmacists()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllPharmacists()
        {
            using (var service = new PharmacistService())
            {
                List<PharmacistModel> result = new List<PharmacistModel>();
                var test = service.GetAll();
                //make a model to hold this
                foreach (var t in test)
                {
                    result.Add(new PharmacistModel(t));
                }
                return Json(result);
            }
        }
    }
}