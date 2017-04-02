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
    //[Authorize(Roles = "Pharmacist")]
    public class ManagePharmacistController : BaseController
    {
        // GET: ManagePharmacist
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SinglePharmacy()
        {
            //int id = User.Pharmacy.Code;
            int id = 1;
            //this id should be grabbed from the user to reflect current
            using (var service = new PharmacyService())
            {
                var result = service.Get(id);
                return View(result);
            }
        }

        [HttpPost]
        public JsonResult GetSinglePharmacist(int id, int PharmacyId)
        {
            using (var service = new PharmacistService())
            {
                var result = service.Get(id);
                return Json(new PharmacistModel(result, PharmacyId));
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
                        new { controller = "ManagePharmacist", action = "SinglePharmacy", Id = PharmacyCode }));
            }
        }
    }
}