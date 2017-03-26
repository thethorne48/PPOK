using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPOK_Twilio.Controllers
{
    //[Authorize(Roles = "SystemAdmin")]
    public class SystemAdminController : BaseController
    {
        // GET: SystemAdmin
        public ActionResult Index()
        {
            var roles = User;
            return View();
        }
    }
}