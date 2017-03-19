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
    public class LandingPageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReturnTable()
        {
            using (var service = new EventService())
            {
                //Make sure to change so that it only gets data where date is in correct time
                //Ask John and Tom to add Date to the database
                var prescription = service.GetAll();
                return PartialView(prescription);
            }
        }

        [HttpPost]
        public ActionResult UpdateDatabase(object file1)
        {
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(file1.ToString()); //this one
            //byte[] byteArray = Encoding.ASCII.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);

            //Upload CSV here
            try
            {
                InitDatabaseService init = new InitDatabaseService();
                init.Reset();
                Pharmacy pharm = new Pharmacy(1, "test1", "test2", "test3");
                using (var service = new PharmacyService())
                {
                    service.Create(pharm);
                }
                init.LoadFromMemoryStream(stream, pharm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            using (var service = new EventService())
            {
            }
            return Redirect("ReturnTable");
        }
    }
}