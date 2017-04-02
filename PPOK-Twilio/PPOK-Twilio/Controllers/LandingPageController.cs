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
    public class LandingPageController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReturnTable()
        {
            using (var service = new EventRefillService())
            {
                //Make sure to change so that it only gets data where date is in correct time
                //Ask John and Tom to add Date to the database
                var prescription = service.GetAll();
                return PartialView(prescription);
            }
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
                init.Reset();
                Pharmacy pharm = User.Pharmacy;
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
    }
}