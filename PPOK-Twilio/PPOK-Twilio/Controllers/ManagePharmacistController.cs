using PPOK.Domain.Models;
using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PPOK_Twilio.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManagePharmacistController : BaseController
    {

        public ActionResult Pharmacy()
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
        public ActionResult EditPharmacist(int Code, int PharmacyCode, string FirstName, string LastName, string Email, string Phone, bool IsActive = false, bool IsAdmin = false)
        {
            Phone = Regex.Replace(Phone, @"[^A-Za-z0-9]+", "");
            if (Phone.Length == 10)
            {
                Phone = "1" + Phone;
            }
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

                    using (var jobservice = new JobService())
                    {
                        //these get the value, not the checked value
                        var job = jobservice.GetWhere(JobService.PharmacistCodeCol == p.Code & JobService.PharmacyCodeCol == PharmacyCode).FirstOrDefault();
                        job.IsActive = IsActive;
                        job.IsAdmin = IsAdmin;
                        jobservice.Update(job);
                    }
                }

                return RedirectToAction("Pharmacy", new RouteValueDictionary(
                        new { controller = "ManagePharmacist", action = "Pharmacy" }));
            }
        }

        [HttpPost]
        public ActionResult AddPharmacist(string FirstName, string LastName, string Email, string Phone, bool IsActive = false, bool IsAdmin = false)
        {
            Phone = Regex.Replace(Phone, @"[^A-Za-z0-9]+", "");
            if (Phone.Length == 10)
            {
                Phone = "1" + Phone;
            }
            using (var service = new PharmacistService())
            {
                Pharmacist p = new Pharmacist(FirstName, LastName, Email, Phone, new byte[] { 0 }, new byte[] { 0 });

                service.Create(p);

                Pharmacy pharm;
                using (var pharmservice = new PharmacyService())
                {
                    pharm = pharmservice.Get(User.getPharmacy().Code);
                }

                using (var jobservice = new JobService())
                {
                    //these get the value, not the checked value
                    Job j = new Job(pharm, p, IsActive, IsAdmin);
                    jobservice.Create(j);
                }

                    return RedirectToAction("Pharmacy", new RouteValueDictionary(
                        new { controller = "ManagePharmacist", action = "Pharmacy", Id = User.Pharmacy.Code }));
            }
        }
    }
}