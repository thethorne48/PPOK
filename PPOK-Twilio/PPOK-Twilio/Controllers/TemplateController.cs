using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPOK.Domain.Service;
using PPOK.Domain.Types;

namespace PPOK_Twilio.Controllers
{
    //TODO get pharmacy for current user instead of just test
    [Authorize(Roles = "Admin")]

    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TemplateTable()
        {
            using (var service = new MessageTemplateService())
            {
                var templates = service.GetAll(new Pharmacy { Code = 1 });
                return PartialView(templates);
            }
        }

        public ActionResult GetContent(MessageTemplateType type, MessageTemplateMedia media)
        {
            using(var service = new MessageTemplateService())
            {
                var template = service.Get(new Pharmacy { Code = 1 }, type, media);
                return Content(template.Content);
            }
        }

        [HttpPost]
        public ActionResult SaveTemplate(MessageTemplateType type, MessageTemplateMedia media, string content)
        {
            using(var service = new MessageTemplateService())
            {
                var template = service.Get(new Pharmacy { Code = 1 }, type, media);
                template.Content = content;
                service.Update(template);
            }
            return null;
        }
    }
}