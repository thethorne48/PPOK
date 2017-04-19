using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPOK.Domain.Service;
using PPOK.Domain.Types;

namespace PPOK_Twilio.Controllers
{
    public class RecallController : BaseController
    {
        // GET: Recall
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Upload()
        {
            List<Patient> p = new List<Patient>();
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var service = new RecallService();
                    using (StreamReader ms = new StreamReader(file.InputStream))
                    {
                        p = service.UploadPatientsFromStream(ms, User.Pharmacy);
                    }                    
                }
            }
            p = p.GetRange(0, p.Count - 1);//because CSV is setting last row to null, strip it out for now to prevent error

            return PartialView("UploadPreview", p);
        }

        [HttpPost]
        public ActionResult Send()
        {
            var x = 3;
            return RedirectToAction("SendConfirmation", new { numPatients = 0 });
        }
    }
}