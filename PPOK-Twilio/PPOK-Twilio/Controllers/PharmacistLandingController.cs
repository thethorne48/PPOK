using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPOK_Twilio.Controllers
{
    public class PharmacistLandingController : Controller
    {
        public ActionResult Index()
        {
            using (var service = new PrescriptionService())
            {
                var prescription = service.GetAll();
                return View(prescription);
            }
        }

        [HttpPost]
        public ActionResult UploadContact(HttpPostedFileBase file)
        {
            //Upload CSV here
            Console.WriteLine("This has worked");

            using (var service = new PrescriptionService())
            {
                var prescription = service.GetAll();
                return PartialView(prescription);
            }
        }
    }
}