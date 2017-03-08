using PPOK_Twilio.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity.Migrations;
using PPOK.Domain.Service;
using PPOK.Domain;
using PPOK.Domain.Types;
using System.Web.Script.Serialization;

namespace PPOK_Twilio.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string email, string password)
        {
            // get redirect
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            using (var service = new PharmacistService())
            {
                Pharmacist pharmacist = service.GetWhere(PharmacistService.EmailCol == email).FirstOrDefault();



                if(pharmacist == null && email != null)
                {

                    PPOKPrincipal.IsValid(email, password);
                    List<Job> jobs = new List<Job>();
                    Pharmacist pharma = new Pharmacist() { Email = email, FirstName = "John", LastName = "Doe", PasswordHash = PPOKPrincipal.HashPassword(password), Phone = "1234567890", Fills = new List<FillHistory>() };
                    jobs.Add(new Job() { IsAdmin = true, IsActive = true, Pharmacist = pharma });
                    pharma.Jobs = jobs;
                    service.Create(pharma);
                    pharmacist = service.GetWhere(PharmacistService.EmailCol == email).FirstOrDefault();
                    //user = new User(service.GetWhere(PharmacistService.EmailCol == email).FirstOrDefault());
                }

                if (pharmacist != null && PPOKPrincipal.IsValid(email, password))
                {
                    PPOKPrincipalSerializeModel serializeModel = new PPOKPrincipalSerializeModel(pharmacist);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string userData = serializer.Serialize(serializeModel);

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, pharmacist.Email, DateTime.Now, DateTime.Now.AddMinutes(3), false, userData);
                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(authCookie);
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View("Index");
                }
            }

            //return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}