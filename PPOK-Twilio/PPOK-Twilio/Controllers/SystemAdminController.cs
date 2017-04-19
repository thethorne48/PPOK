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
using static PPOK.Domain.Utility.Config;

namespace PPOK_Twilio.Controllers
{
    [Authorize(Roles = "System")]
    public class SystemAdminController : Controller
    {
        private static string newAccountEmailSubject = "Set up your new PPOkTwilio Account";
        private static string newAccountEmailBody = "Hello,<br> please <a href=\"" + ExternalUrl + "/Account/Patient" +
                    "\">go to the login page</a> and click 'Forgot Password', and enter your email to set up your new account.";

        // GET: SystemAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pharmacists()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Admins()
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
        public ActionResult AdminModal()
        {
            return PartialView("_AdminModal");
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
        public JsonResult Inactivate(int PharmacyId)
        {
            using (var service = new PharmacyService())
            {
                var result = service.Get(PharmacyId);
                using (var pservice = new JobService())
                {
                    foreach (var pharmacist in result.Jobs)
                    {
                        pharmacist.IsActive = false;
                        pservice.Update(pharmacist);
                    }
                }
                return Json(true);
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
        public JsonResult GetSingleAdmin(int id)
        {
            using (var service = new SystemAdminService())
            {
                var result = service.Get(id);
                return Json(result);
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
        public ActionResult AddPharmacist(int PharmacyCode, string FirstName, string LastName, string Email, string Phone, bool IsAdmin = false, bool IsActive = false)
        {
            using (var service = new PharmacistService())
            {
                Phone = Regex.Replace(Phone, @"[^A-Za-z0-9]+", "");
                if (Phone.Length == 10)
                {
                    Phone = "1" + Phone;
                }
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
            }
            using (var service = new EmailService())
            {
                service.SendEmail(Email, newAccountEmailSubject, newAccountEmailBody);
            }
            return RedirectToAction("SinglePharmacy", new RouteValueDictionary(
                        new { controller = "SystemAdmin", action = "SinglePharmacy", Id = PharmacyCode }));
        }
        [HttpPost]
        public ActionResult AddPharmacy(string Name, string Address, string Phone)
        {
            Phone = Regex.Replace(Phone, @"[^A-Za-z0-9]+", "");
            if (Phone.Length == 10)
            {
                Phone = "1" + Phone;
            }
            using (var service = new PharmacyService())
            {
                int Code = service.GetAll().Max(x => x.Code); //this is an issue with the db where the code is auto set to 0 when creating a pharmacy
                Pharmacy p = new Pharmacy(++Code, Name, Phone, Address);

                service.Create(p);

            }
            return RedirectToAction("PharmacyView", new RouteValueDictionary(
                    new { controller = "SystemAdmin", action = "PharmacyView" }));
        }

        [HttpPost]
        public ActionResult EditPharmacy(int PharmacyCode, string Name, string Address, string Phone)
        {
            Phone = Regex.Replace(Phone, @"[^A-Za-z0-9]+", "");
            if (Phone.Length == 10)
            {
                Phone = "1" + Phone;
            }
            using (var service = new PharmacyService())
            {
                Pharmacy p = service.Get(PharmacyCode);
                p.Name = Name;
                p.Address = Address;
                p.Phone = Phone;
                service.Update(p);
            }
            return RedirectToAction("SinglePharmacy", new RouteValueDictionary(
                       new { controller = "SystemAdmin", action = "SinglePharmacy", Id = PharmacyCode }));
        }

        [HttpPost]
        public ActionResult EditAdmin(int Code, string FirstName, string LastName, string Email, string Phone)
        {
            Phone = Regex.Replace(Phone, @"[^A-Za-z0-9]+", "");
            if (Phone.Length == 10)
            {
                Phone = "1" + Phone;
            }
            using (var service = new SystemAdminService())
            {
                SystemAdmin p = service.Get(Code);
                p.FirstName = FirstName;
                p.LastName = LastName;
                p.Email = Email;
                p.Phone = Phone;
                service.Update(p);
            }
            return RedirectToAction("Admins", new RouteValueDictionary(
                       new { controller = "SystemAdmin", action = "Admins" }));
        }
        [HttpPost]
        public ActionResult EditForAllPharmacist(int PharmacistCode, int PharmacyCode, string FirstName, string LastName, string Email, string Phone, bool IsAdmin = false, bool IsActive = false)
        {
            Phone = Regex.Replace(Phone, @"[^A-Za-z0-9]+", "");
            if (Phone.Length == 10)
            {
                Phone = "1" + Phone;
            }
            using (var service = new PharmacistService())
            {
                Pharmacist p = service.Get(PharmacistCode);
                if (p != null)
                {
                    p.FirstName = FirstName;
                    p.LastName = LastName;
                    p.Phone = Phone;
                    p.Email = Email;
                    var temp1 = p.AllJobs.Where(x => x.Pharmacy.Code == PharmacyCode).FirstOrDefault();
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
        public ActionResult EditPharmacist(int PharmacistCode, int PharmacyCode, string FirstName, string LastName, string Email, string Phone, bool IsAdmin = false, bool IsActive = false)
        {
            Phone = Regex.Replace(Phone, @"[^A-Za-z0-9]+", "");
            if (Phone.Length == 10)
            {
                Phone = "1" + Phone;
            }
            using (var service = new PharmacistService())
            {
                Pharmacist p = service.Get(PharmacistCode);
                if (p != null)
                {
                    p.FirstName = FirstName;
                    p.LastName = LastName;
                    p.Phone = Phone;
                    p.Email = Email;
                    var temp1 = p.AllJobs.Where(x => x.Pharmacy.Code == PharmacyCode).FirstOrDefault();
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
        public JsonResult GetAllAdmins()
        {
            using (var service = new SystemAdminService())
            {
                List<SystemAdminModel> result = new List<SystemAdminModel>();
                var admins = service.GetAll();
                foreach (var admin in admins)
                {
                    result.Add(new SystemAdminModel(admin));
                }
                return Json(result);
            }
        }
        [Authorize(Roles = "System")]
        [HttpPost]
        public ActionResult AddAdmin(string FirstName, string LastName, string Email, string Phone)
        {
            Phone = Regex.Replace(Phone, @"[^A-Za-z0-9]+", "");
            if (Phone.Length == 10)
            {
                Phone = "1" + Phone;
            }
            using (var emailService = new EmailService())
            using (var service = new SystemAdminService())
            {
                service.Create(new SystemAdmin(FirstName, LastName, Email, Phone, new byte[0], new byte[0]));
                emailService.SendEmail(Email, newAccountEmailSubject, newAccountEmailBody);
                return View("Admins");
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