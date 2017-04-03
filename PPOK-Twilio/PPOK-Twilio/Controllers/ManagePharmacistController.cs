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
    [Authorize(Roles = "Pharmacist")]
    public class ManagePharmacistController : BaseController
    {
        // GET: ManagePharmacist
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SinglePharmacy()
        {
            int id = User.Pharmacy.Code;
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

        [HttpPost]
        public ActionResult AddPharmacist(string FirstName, string LastName, string Email, string Phone)
        {
            using (var service = new PharmacistService())
            {
                Pharmacist p = new Pharmacist(FirstName, LastName, Email, Phone, new byte[] { 0 }, new byte[] { 0 });

                service.Create(p);

                Pharmacy pharm;
                using (var pharmservice = new PharmacyService())
                {
                    pharm = pharmservice.Get(User.Pharmacy.Code);
                }

                using (var jobservice = new JobService())
                {
                    Job j = new Job(pharm, p, true, false);
                    jobservice.Create(j);
                }

                    return RedirectToAction("SinglePharmacy", new RouteValueDictionary(
                        new { controller = "ManagePharmacist", action = "SinglePharmacy", Id = User.Pharmacy.Code }));
            }
        }
    }
}