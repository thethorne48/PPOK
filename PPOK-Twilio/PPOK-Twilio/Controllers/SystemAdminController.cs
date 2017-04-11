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
        public JsonResult GetSinglePharmacistForAllPharm(int id, int PharmacyId)
        {
            using (var service = new PharmacistService())
            {
                var result = service.Get(id);
                return Json(new PharmacistModel(result, PharmacyId));
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
        public JsonResult GetAllPharmacists()
        {
            using (var service = new PharmacistService())
            {
                List<PharmacistModel> result = new List<PharmacistModel>();
                var test = service.GetAll();
                //make a model to hold this
                foreach (var t in test)
                {
                    var temp = new PharmacistModel(t);
                    result.Add(temp);
                }
                return Json(result);
            }
        }
        [HttpPost]
        public ActionResult AddPharmacist(int PharmacyCode, string FirstName, string LastName, string Email, string Phone, bool IsAdmin = false, bool IsActive=false)
        {
            using (var service = new PharmacistService())
            {
                Pharmacist p = new Pharmacist(FirstName, LastName, Email, Phone, new byte[] { 0 }, new byte[] { 0 });
                
                service.Create(p);

                Pharmacy pharm;
                using (var pharmservice = new PharmacyService())
                {
                    pharm = pharmservice.Get(PharmacyCode);
                }

                using (var jobservice = new JobService())
                {
                    Job j = new Job(pharm, p, true, false);
                    j.IsAdmin = IsAdmin;
                    j.IsActive = IsActive;
                    jobservice.Create(j);
                }

                return RedirectToAction("SinglePharmacy", new RouteValueDictionary(
                        new { controller = "SystemAdmin", action = "SinglePharmacy", Id = PharmacyCode }));
            }
        }
        [HttpPost]
        public ActionResult EditForAllPharmacist(int PharmacistCode, int PharmacyCode, string FirstName, string LastName, string Email, string Phone, bool IsAdmin=false, bool IsActive=false)
        {
            using (var service = new PharmacistService())
            {
                Pharmacist p = service.Get(PharmacistCode);
                if (p != null)
                {
                    p.FirstName = FirstName;
                    p.LastName = LastName;
                    p.Phone = Phone;
                    p.Email = Email;
                    var temp1 = p.Jobs.Where(x => x.Pharmacy.Code == PharmacyCode).FirstOrDefault();
                    using (var serviceJob = new JobService())
                    {
                        var j = serviceJob.GetWhere(JobService.CodeCol == temp1.Code).FirstOrDefault();
                        j.IsActive = IsActive;
                        j.IsAdmin = IsAdmin;
                        serviceJob.Update(j);
                    }
                    service.Update(p);
                }
                return RedirectToAction("Pharmacists", new RouteValueDictionary(
                        new { controller = "SystemAdmin", action = "Pharmacists" }));
            }
        }
        [HttpPost]
        public ActionResult EditPharmacist(int PharmacistCode, int PharmacyCode, string FirstName, string LastName, string Email, string Phone,bool IsAdmin = false, bool IsActive = false)
        {
            using (var service = new PharmacistService())
            {
                Pharmacist p = service.Get(PharmacistCode);
                if (p != null)
                {
                    p.FirstName = FirstName;
                    p.LastName = LastName;
                    p.Phone = Phone;
                    p.Email = Email;
                    var temp1 = p.Jobs.Where(x => x.Pharmacy.Code == PharmacyCode).FirstOrDefault();
                    using (var serviceJob = new JobService())
                    {
                        var j = serviceJob.GetWhere(JobService.CodeCol == temp1.Code).FirstOrDefault();
                        j.IsActive = IsActive;
                        j.IsAdmin = IsAdmin;
                        serviceJob.Update(j);
                    }
                    
                        service.Update(p);
                }
                return RedirectToAction("SinglePharmacy", new RouteValueDictionary(
                        new { controller = "SystemAdmin", action = "SinglePharmacy", Id = PharmacyCode }));
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