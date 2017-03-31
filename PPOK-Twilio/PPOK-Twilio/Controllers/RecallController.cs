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
    public class RecallController : Controller
    {
        // GET: Recall
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            string path = "";
            List<Patient> p = new List<Patient>();
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = "Recall.csv";// Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/App_Data/Temp"), fileName);

                    RecallService rs = new RecallService();
                    p = rs.UploadPatients(path);
                    
                }
            }
            //limit preview results
            if (p.Count > 5)
            {
                p = p.GetRange(0, 5);
            }

            return PartialView("UploadPreview", p);
        }

        //public ActionResult Get
    }
}