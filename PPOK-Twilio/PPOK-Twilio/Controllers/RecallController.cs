﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPOK.Domain.Service;
using PPOK.Domain.Types;
using PPOK.Domain.Models;

namespace PPOK_Twilio.Controllers
{
    [Authorize(Roles = "Admin")]
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
            return PartialView("UploadPreview", p);
        }

        [HttpPost]
        public ActionResult Send()
        {
            SendRecallModel m = new SendRecallModel()
            {
                PatientCodesCsv = Request.Form["PatientCodesCsv"],
                TemplateBody = Request.Form["TemplateBody"]
            };

            

            List<MessageTemplate> recallTemplates;
            using (var service = new MessageTemplateService())
            {
                recallTemplates = service.GetWhere(MessageTemplateService.PharmacyCodeCol == User.Pharmacy.Code & MessageTemplateService.TypeCol == MessageTemplateType.RECALL);

                foreach (MessageTemplate t in recallTemplates)
                {
                    t.Content = m.TemplateBody;
                    service.Update(t);
                }
            }

            string[] codes = m.PatientCodesCsv.Split(',');
            List<Event> eventsCreated = new List<Event>();
            foreach (string code in codes)
            {
                Patient p = null;
                using (var service = new PatientService())
                {
                    p = service.Get(code);
                }
                if (p != null)
                {
                    Event e = new Event
                    {
                        Status = EventStatus.ToSend,
                        Patient = p,
                        Type = EventType.RECALL
                    };
                    using (var service = new EventService())
                    {
                        service.Create(e);
                    }
                    eventsCreated.Add(e);
                    EventRecall er = new EventRecall
                    {
                        Event = e
                    };
                    using (var service = new EventRecallService())
                    {
                        service.Create(er);
                    }
                }
            }
            EventProcessingService.SendEvents(eventsCreated, recallTemplates);

            return RedirectToAction("SendConfirmation", new { numSent = eventsCreated.Count });
        }
    }
}
 