using PPOK.Domain.Models;
using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PPOK_Twilio.Controllers
{
    [Authorize(Roles = "Pharmacist Admin")]
    public class LandingPageController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReturnTable()
        {
            List<DayEventModel> models = new List<DayEventModel>();
            using(var service = new EventScheduleService())
            {
                //get all scheduled events where the date is today
                models = service.GetEventsBeforeToday()
                    .Select(e => new DayEventModel(e))
                    .ToList();
            }
            return PartialView(models);
        }

        [HttpPost]
        public ActionResult UpdateDatabase(string file1)
        {
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(file1); //this one
            //byte[] byteArray = Encoding.ASCII.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);

            //Upload CSV here
            try
            {
                InitDatabaseService init = new InitDatabaseService();
                //init.Reset();
                //this is bad, we need to check for duplicates, not drop the tables
                Pharmacy pharm = User.getPharmacy();
                init.LoadFromMemoryStream(stream, pharm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(ex);
            }

            return null;
        }

        [HttpPost]
        public ActionResult RemoveEventScheduleByID(int id)
        {
            using (var service = new EventScheduleService())
            {
                service.Delete(id);
            }
            return null;
        }

        public JsonResult Send()
        {
            using (var service = new EventScheduleService())
            {
                int pharmacyCode = User.Pharmacy.Code;
                //get all scheduled events where the date is today
                EventProcessingService.SendEvents(service.GetEventsForToday(), pharmacyCode);
            }
            return Json(true);
        }
    }
}