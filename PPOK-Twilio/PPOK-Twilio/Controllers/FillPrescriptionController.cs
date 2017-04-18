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
    public class FillPrescriptionController : BaseController
    {
        // GET: Fill
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Fill(int id)
        {
            Pharmacist pharm = new Pharmacist();
            using (var pharService = new PharmacistService())
            {
                pharm = pharService.Get(User.Code);
            }
            using (var service = new EventService())
            {
                var Er = service.Get(id);
                using (var fillservice = new FillHistoryService())
                {
                    FillHistory history = new FillHistory(Er.Refills.FirstOrDefault(), pharm, DateTime.Now);
                    fillservice.Create(history);
                }
                EventProcessingService.SendEvent(Er, User.Pharmacy.Code);
                return Json(true);
            }
        }

        [HttpPost]
        public JsonResult GetAllFilledPrescriptions()
        {
            using (var service = new EventService())
            {

                var test = service.GetWhere(EventService.StatusCol == EventStatus.Fill);
                List<FillModel> result = new List<FillModel>();
                foreach (var t in test)
                {
                    //if(t.Prescription.Patient.Pharmacy == User.Pharmacy)
                    var temp = new FillModel(t.Refills.FirstOrDefault());
                    if (temp != null)
                        result.Add(temp);
                }

                return Json(result);
            }
        }
    }
}