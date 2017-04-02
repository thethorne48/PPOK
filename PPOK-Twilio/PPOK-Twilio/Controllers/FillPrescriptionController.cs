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
            using (var service = new EventService())
            {
                //need to just inactivate
                var fill = service.Get(id);
                var temp = fill.History.ToList();
                FillHistory history = new FillHistory();
                history.Date = DateTime.Now;
                //need help
                //history.Event = fill;
                //history.Status = EventStatus.Sent;
                //temp.Add(history);
                //fill.History = temp;
                using (var service1 = new FillHistoryService())
                {
                    service1.Update(history);
                }
                service.Update(fill);
                return Json(true);
            }
        }

        [HttpPost]
        public JsonResult GetAllFilledPrescriptions()
        {
            using (var service = new EventService())
            {
                List<FillModel> result = new List<FillModel>();
                var test = service.GetAll(); //need to get based on pharmacy for most things
                foreach (var q in test)
                    result.Add(new FillModel(q));
                //var test = service.GetAll().Where(); //get all that are not inactive
                ////make a model to hold this
                //foreach (var t in test)
                //{
                //    result.Add(new FillModel(t));
                //}
                return Json(result);
            }
        }
    }
}