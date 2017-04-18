using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPOK.Domain.Service;
using PPOK.Domain.Types;

namespace PPOK_Twilio.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TemplateController : BaseController
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
                var templates = service.GetAll(User.Pharmacy);
                return PartialView(templates);
            }
        }

        public ActionResult GetContent(MessageTemplateType type, MessageTemplateMedia media)
        {
            using(var service = new MessageTemplateService())
            {
                var template = service.Get(User.Pharmacy, type, media);
                return Content(template.Content);
            }
        }

        [HttpPost]
        public ActionResult SaveTemplate(MessageTemplateType type, MessageTemplateMedia media, string content)
        {
            using(var service = new MessageTemplateService())
            {
                var template = service.Get(User.Pharmacy, type, media);
                template.Content = content;
                service.Update(template);
            }
            return null;
        }
    }
}