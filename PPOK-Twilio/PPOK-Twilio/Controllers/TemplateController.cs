using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPOK.Domain.Service;
using PPOK.Domain.Types;

namespace PPOK_Twilio.Controllers
{
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
                var templates = service.GetAll();
                return PartialView(templates);
            }
        }

        public ActionResult GetContent(MessageTemplateType type)
        {
            using(var service = new MessageTemplateService())
            {
                var template = service.Get(type);
                return Content(template.Content);
            }
        }

        [HttpPost]
        public ActionResult SaveTemplate(MessageTemplateType type, string content)
        {
            using(var service = new MessageTemplateService())
            {
                var template = service.Get(type);
                template.Content = content;
                service.Update(template);
            }
            return null;
        }
    }
}