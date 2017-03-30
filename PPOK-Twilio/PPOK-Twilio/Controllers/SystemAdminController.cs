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

        public ActionResult PharmacyView()
        {
            return View();
        }
        public ActionResult SinglePharmacy(int id)
        {
            using (var service = new PharmacyService())
            {
                var result = service.Get(id);
                return View(result);
            }
        }

        [HttpPost]
        public JsonResult GetSinglePharmacist(int id,int PharmacyId)
        {
            using (var service = new PharmacistService())
            {
                var result = service.Get(id);
                return Json(new PharmacistModel(result, PharmacyId));
            }
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
        [HttpPost]
        public ActionResult EditPharmacist(int Code, int PharmacyCode, string FirstName, string LastName, string Email, string Phone)
        {
            using (var service = new PharmacistService())
            {
                Pharmacist p = service.Get(Code);
                if (p != null)
                {
                    p.FirstName = FirstName;
                    p.LastName = LastName;
                    p.Phone = Phone;
                    p.Email = Email;
                    service.Update(p);
                }
                return RedirectToAction("SinglePharmacy", new RouteValueDictionary(
                        new { controller = "SystemAdmin", action = "SinglePharmacy", Id = PharmacyCode }));
                //return RedirectToAction("SinglePharmacy","SystemAdmin",PharmacyCode);
            }
        }
        [HttpPost]
        public JsonResult GetAllPharmacies()
        {
            using (var service = new PharmacyService())
            {
                List<PharmacyModel> result = new List<PharmacyModel>();
                var test = service.GetAll();
                //make a model to hold this
                foreach (var t in test)
                {
                    result.Add(new PharmacyModel(t));
                }
                return Json(result);
            }
        }
    }
}