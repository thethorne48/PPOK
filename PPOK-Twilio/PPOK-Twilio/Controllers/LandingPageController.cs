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
            List<DayEventModel> d = new List<DayEventModel>();
            using (var service = new EventRefillService())
            {
                //Make sure to change so that it only gets data where date is in correct time
                //Ask John and Tom to add Date to the database
                var prescription = service.GetAll();
                foreach (var t in prescription)
                    d.Add(new DayEventModel(t));
                
            }
            using (var service = new EventBirthdayService())
            {
                //Make sure to change so that it only gets data where date is in correct time
                //Ask John and Tom to add Date to the database
                var prescription = service.GetAll();
                foreach (var t in prescription)
                    d.Add(new DayEventModel(t));
            }
            return PartialView(d);
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
                using (var service = new PharmacyService())
                {
                    service.Create(pharm);
                }
                init.LoadFromMemoryStream(stream, pharm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(ex);
            }

            return null;
        }

        public JsonResult Send()
        {
            using (var service = new EventBirthdayService())
            {
                var t = service.GetAll();
                foreach(var l in t)
                    CommunicationsService.Send(l);
            }
            using (var service = new EventRefillService())
            {
                var t = service.GetAll();
                foreach (var l in t)
                    CommunicationsService.Send(l);
            }
            return Json(true);
        }
    }
}