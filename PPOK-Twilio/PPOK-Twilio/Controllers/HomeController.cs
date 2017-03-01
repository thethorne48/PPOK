using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PPOK_Twilio.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult SendEmail()
        {
            ViewBag.Message = "Your application description page.";
            PPOK.Domain.Service.SendEmailService service = new PPOK.Domain.Service.SendEmailService();
            service.Create("somerandomninjaguy@gmail.com");

            return View("Index");
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}