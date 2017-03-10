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

                return View(patient);
            }
        }
    }
}