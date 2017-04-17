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
    [Authorize(Roles = "Pharmacist Admin")]
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
            using (var service = new EventRefillService())
            {
                var Er = service.Get(id);
                using (var fillservice = new FillHistoryService())
                {
                    FillHistory history = new FillHistory(Er, pharm, DateTime.Now);
                    fillservice.Create(history);
                }
                //Er.Prescription.Patient.Email = "emily.pielemeier@eagles.oc.edu";
                //Er.Prescription.Patient.Phone = "3177536066";
                CommunicationsService.Send(Er);
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