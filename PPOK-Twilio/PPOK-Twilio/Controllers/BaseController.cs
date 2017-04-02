using PPOK_Twilio.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPOK_Twilio.Controllers
{
    public class BaseController : Controller
    {
        protected virtual new PPOKPrincipal User
        {
            get
            {
                return base.User as PPOKPrincipal;
            }
        }
    }
}