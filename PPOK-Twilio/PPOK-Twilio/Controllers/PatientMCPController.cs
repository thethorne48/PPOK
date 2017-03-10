using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPOK_Twilio.Controllers
{
    public class PatientMCPController : Controller
    {
        // GET: PatientMCP
        public ActionResult Index()
        {
            using (var service = new PatientService())
            {
                var patient = service.GetWhere(PatientService.EmailCol == "christopher.sartin@eagles.oc.edu").FirstOrDefault();
                //Patient patient = new Patient();
                //patient.FirstName = "Christopher";
                //patient.LastName = "Sartin";
                //patient.ContactPreference = ContactPreference.PHONE;
                //patient.Phone = "918-399-4836";
                //patient.Email = "christopher.sartin@eagles.oc.edu";

                return View(patient);
            }
        }
    }
}