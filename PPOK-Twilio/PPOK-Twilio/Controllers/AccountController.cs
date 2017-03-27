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
using PPOK.Domain.Models;
using System.Web.Script.Serialization;
using PPOK.Domain.Utility;

namespace PPOK_Twilio.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        [HttpGet]
        public ActionResult Index()
        {
            return View(new LoginModel());
        }


        [HttpGet]
        public ActionResult Patient()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult SendCode(int pharmacy, string email)
        {
            using (var service = new PatientService())
            {
                var patient = service.GetWhere(PatientService.EmailCol == email & PatientService.PharmacyCodeCol == pharmacy).FirstOrDefault();
                if (patient != null)
                    return RedirectToAction("Index", "PatientMCP", new { patientCode = patient.Code });
                else
                    return View("Patient");
            }
        }

        [HttpPost]
        public ActionResult Login(string username, string password, int pharmacy = -2)
        {
            // get page to redirect to
            if (!ModelState.IsValid)
            {
                return View("Index", new LoginModel());
            }

            //var user = User.Identity
            using (var PharmService = new PharmacistService())
            using (var PatService = new PatientService())
            using (var SysService = new SystemAdminService())
            {
                // add a system admin if it isnt there
                //if (SysService.GetWhere(SystemAdminService.EmailCol == "luke.thorne@eagles.oc.edu").FirstOrDefault() == null)
                //{
                //    SysService.Create(new SystemAdmin() { FirstName = "Luke", LastName = "Thorne", Email = "luke.thorne@eagles.oc.edu", PasswordHash = PPOKPrincipal.HashPassword("password") });
                //}

                // TODO: Make the login work for system admins so we can add pharmacies and pharmacists
                Pharmacist pharmacist = PharmService.GetWhere(PharmacistService.EmailCol == username).FirstOrDefault();
                //Patient patient = PatService.GetWhere(PatientService.PhoneCol == username).FirstOrDefault();
                SystemAdmin admin = SysService.GetWhere(SystemAdminService.EmailCol == username).FirstOrDefault();
                //if (pharmacist == null && username != null)
                //{
                //    List<Job> jobs = new List<Job>();
                //    Pharmacist pharma = new Pharmacist() { Email = username, FirstName = username, LastName = "Not Set", PasswordHash = PPOKPrincipal.HashPassword(password), Phone = "1234567890", Fills = new List<FillHistory>() };
                //    jobs.Add(new Job() { IsAdmin = true, IsActive = true, Pharmacist = pharma });
                //    using (var jobService = new JobService())
                //    {
                //        foreach (var job in jobs)
                //        {
                //            //jobService.Create(job);
                //        }
                //    }
                //    pharma.Jobs = jobs;
                //    PharmService.Create(pharma);
                //    pharmacist = PharmService.GetWhere(PharmacistService.EmailCol == username).FirstOrDefault();
                //    //user = new User(service.GetWhere(PharmacistService.EmailCol == email).FirstOrDefault());
                //}


                if (admin != null && PPOKPrincipal.IsValid(admin.Email, password) && pharmacy == -1)
                {
                    var serializedAdmin = new PPOKPrincipalSerializeModel(admin);
                    serializedAdmin.AddToRole("System");
                    makeAuthTicket(serializedAdmin);
                    return RedirectToAction("Index", "Home");
                }
                else if (pharmacist != null && PPOKPrincipal.IsValid(username, password) && pharmacy != -1)
                {
                    using (var service = new JobService())
                    {
                        using (var pharmacyService = new PharmacyService())
                        {
                            pharmacist.Jobs = service.GetWhere(JobService.PharmacistCodeCol == pharmacist.Code & JobService.PharmacyCodeCol == pharmacy);
                        }
                    }
                    using (var service = new FillHistoryService())
                    {
                        pharmacist.Fills = service.GetWhere(FillHistoryService.PharmacistCodeCol == pharmacist.Code);
                    }
                    var serializedPharmacist = new PPOKPrincipalSerializeModel(pharmacist);
                    foreach (var job in pharmacist.Jobs)
                    {

                        if(job.IsActive)
                        {
                            if(job.IsAdmin)
                                serializedPharmacist.AddToRole("Admin");
                            serializedPharmacist.AddToRole("Pharmacist");
                        }
                    }
                    makeAuthTicket(serializedPharmacist);
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View("Index", new LoginModel());
                }
            }
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            using (var service = new PharmacistService())
            {
                var pharmacist = service.GetWhere(PharmacistService.EmailCol == email).FirstOrDefault();
                if (pharmacist != null)
                {
                    using (var emailService = new EmailService(Config.BotEmail, Config.BotPassword))
                    {
                        var id = Convert.ToBase64String(PPOKPrincipal.HashUserText(pharmacist.Code, email));
                        var urlEncoded = HttpUtility.UrlEncode(id);
                        var link = "<a href='" + Request.Url.Authority + "/Account/ResetPassword?code=" + pharmacist.Code + "&identifier=" + urlEncoded + "'> Link </a>";
                        emailService.SendEmail(Config.BotEmail, email, "Password Reset Request", "You have requested to reset your password, good luck. Follow the following link: " + link);
                    }
                    return View("Index", new LoginModel());
                }
                else
                {
                    ViewBag.Error = "That email was not found";
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult ResetPassword(int code, string identifier)
        {
            using (var service = new PharmacistService())
            {
                var pharmacist = service.Get(code);
                var hash = Convert.ToBase64String(PPOKPrincipal.HashUserText(code, pharmacist.Email));
                if (Convert.ToBase64String(PPOKPrincipal.HashUserText(code, pharmacist.Email)) == identifier)
                {
                    return View();
                }
            }
            ViewBag.Error = "That link was not correct. Try again using a new link.";
            return View("ForgotPassword");
        }

        [HttpPost]
        public ActionResult ResetPassword(int code, string identifier, string password)
        {
            using (var service = new PharmacistService())
            {
                var pharmacist = service.Get(code);
                if (Convert.ToBase64String(PPOKPrincipal.HashUserText(code, pharmacist.Email)) == HttpUtility.UrlDecode(identifier))
                {
                    pharmacist.PasswordHash = PPOKPrincipal.HashPassword(password);
                    service.Update(pharmacist);
                    return View("Index", new LoginModel());
                }
                else
                {
                    ViewBag.Error = "That link was not correct. Try again using a new link.";
                    return View("ForgotPassword");
                }
            }
        }

        [HttpGet]
        public ActionResult CheckPassword(string password)
        {
            // doesnt work yet
            return Json(PPOKPrincipal.passwordComplexity(password));
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private void makeAuthTicket(PPOKPrincipalSerializeModel user)
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            user.Pharmacy.Jobs = null;
            user.Pharmacy.Patients = null;
            string userData = serializer.Serialize(user);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(3), false, userData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(authCookie);
        }
    }
}