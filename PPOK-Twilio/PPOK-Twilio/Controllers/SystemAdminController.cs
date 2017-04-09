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
        public JsonResult Inactivate(int id, int PharmacyId)
        {
            //IMPLEMENT THIS, should be pretty simple i think

            //Pharmacist pharm = new Pharmacist();
            //string eventType = null;
            //using (var pharService = new PharmacistService())
            //{
            //    pharm = pharService.Get(User.Code);
            //}
            //using (var eservice = new EventService())
            //{
            //    var temp = eservice.Get(id);
            //    if (temp.Refills.FirstOrDefault() != null)
            //    {
            //        eventType = "Refill Event";
            //    }
            //    else if (temp.Birthdays.FirstOrDefault() != null)
            //    {
            //        eventType = "Birthday Event";
            //    }
            //    else
            //        eventType = "Recall Event";
            //}
            //if (eventType == "Refill Event")
            //{
            //    using (var service = new EventRefillService())
            //    {
            //        var Er = service.Get(id);
            //        using (var historyService = new EventHistoryService())
            //        {
            //            EventHistory Eh = new EventHistory(Er.Event, EventStatus.InActive, DateTime.Now);
            //            historyService.Create(Eh);
            //        }
            //        using (var eventService = new EventService())
            //        {
            //            var up = eventService.Get(Er.Event.Code);
            //            up.Status = EventStatus.InActive;
            //            eventService.Update(up);
            //        }
            //    }
            //}
            //else if (eventType == "Recall Event")
            //{
            //    using (var service = new EventRecallService())
            //    {
            //        var Er = service.Get(id);
            //        using (var historyService = new EventHistoryService())
            //        {
            //            EventHistory Eh = new EventHistory(Er.Event, EventStatus.InActive, DateTime.Now);
            //            historyService.Create(Eh);
            //        }
            //        using (var eventService = new EventService())
            //        {
            //            var up = eventService.Get(Er.Event.Code);
            //            up.Status = EventStatus.InActive;
            //            eventService.Update(up);
            //        }
            //    }
            //}
            //else if (eventType == "Birthday Event")
            //{
            //    using (var service = new EventBirthdayService())
            //    {
            //        var Er = service.Get(id);
            //        using (var historyService = new EventHistoryService())
            //        {
            //            EventHistory Eh = new EventHistory(Er.Event, EventStatus.InActive, DateTime.Now);
            //            historyService.Create(Eh);
            //        }
            //        using (var eventService = new EventService())
            //        {
            //            var up = eventService.Get(Er.Event.Code);
            //            up.Status = EventStatus.InActive;
            //            eventService.Update(up);
            //        }
            //    }
            //}
            return Json(true);

        }


        [HttpPost]
        public ActionResult AddPharmacist(string FirstName, string LastName, string Email, string Phone, string PharmacyName)
        {
            int PharmacyCode = 0; //derive this from the Pharmacy string
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
                    jobservice.Create(j);
                }
                return RedirectToAction("Pharmacists", new RouteValueDictionary(
                    new { controller = "SystemAdmin", action = "Pharmacists" }));
            }
        }
        [HttpPost]
        public ActionResult EditForAllPharmacist(int Code, int PharmacyCode, string FirstName, string LastName, string Email, string Phone)
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
                return RedirectToAction("Pharmacists", new RouteValueDictionary(
                        new { controller = "SystemAdmin", action = "Pharmacists" }));
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
            }
        }
        [HttpPost]
        public ActionResult AddPharmacist(int PharmacyCode, string FirstName, string LastName, string Email, string Phone)
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
                    jobservice.Create(j);
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