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
            //using (var service = new PrescriptionService())
            //{
            //    var prescription = service.GetAll();
            //    return View();
            //}

            var prescription = new Prescription();
            prescription.Patient = new Patient();
            prescription.Patient.FirstName = "Christopher";
            prescription.Patient.LastName = "Sartin";
            prescription.Drug = new Drug();
            prescription.Drug.Name = "Drug";
            prescription.Patient.Phone = "867-5309";

            var pass = new List<Prescription>();
            pass.Add(prescription);

            return View(pass);
        }
    }
}