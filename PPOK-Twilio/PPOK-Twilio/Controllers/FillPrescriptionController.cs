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
            //create a fill history to the prescription
            //create an eventhistory
            //send message to the user
            //use the dang constructos
            Pharmacist pharm = new Pharmacist();
            using (var pharService = new PharmacistService())
            {
                //pharm = pharService.Get(User.Code);
                pharm = pharService.Get(1);

            }
            using (var service = new EventRefillService())
            {
                var Er = service.Get(id);
                using (var fillservice = new FillHistoryService())
                {
                    FillHistory history = new FillHistory(Er, pharm,DateTime.Now);
                    fillservice.Create(history);
                }
                using (var historyService = new EventHistoryService())
                {
                    EventHistory Eh = new EventHistory(Er.Event, EventStatus.Complete, DateTime.Now);
                    historyService.Create(Eh);
                }
                //SEND A MESSAGE HERE
                return Json(true);
            }
        }

        [HttpPost]
        public JsonResult GetAllFilledPrescriptions()
        {
            using (var service = new EventRefillService())
            {
                var test = service.GetAll(); //ask jon how to make the where
                //this grabs everything, need it to just grab ones that are still needing to be filled
                List<FillModel> result = new List<FillModel>();
                foreach (var t in test)
                {
                    //if(t.Prescription.Patient.Pharmacy == User.Pharmacy)
                        result.Add(new FillModel(t));
                }

                return Json(result);
            }
        }
    }
}