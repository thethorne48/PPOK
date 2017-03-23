using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = "Recall.csv";// Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/App_Data/Temp"), fileName);
                    file.SaveAs(path);
                }
            }

            return Json(new { fileName = path });
        }
    }
}