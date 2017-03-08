using PPOK_Twilio.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PPOK_Twilio.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //if (Request.IsAuthenticated)
            //{
            //    var me = User.Email;
            //}
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult SendEmail()
        {
            //this is simply a test function to show off send email functionality
            ViewBag.Message = "Your application description page.";
            PPOK.Domain.Service.SendEmailService service = new PPOK.Domain.Service.SendEmailService();
            service.Create("somerandomninjaguy@gmail.com", DateTime.Now);

            return View("Index");
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}