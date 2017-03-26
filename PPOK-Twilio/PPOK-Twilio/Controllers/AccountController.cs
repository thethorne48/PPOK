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
using PPOK_Twilio.Models;
using System.Web.Script.Serialization;

namespace PPOK_Twilio.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Patient()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // get page to redirect to
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            //var user = User.Identity
            using (var PharmService = new PharmacistService())
            using (var PatService = new PatientService())
            using (var SysService = new SystemAdminService())
            {
                // add a system admin if it isnt there
                if (SysService.GetWhere(SystemAdminService.EmailCol == "luke.thorne@eagles.oc.edu").FirstOrDefault() == null)
                {
                    SysService.Create(new SystemAdmin() { FirstName = "Luke", LastName = "Thorne", Email = "luke.thorne@eagles.oc.edu", PasswordHash = PPOKPrincipal.HashPassword("password") });
                }

                // TODO: Make the login work for system admins so we can add pharmacies and pharmacists
                Pharmacist pharmacist = PharmService.GetWhere(PharmacistService.EmailCol == username).FirstOrDefault();
                //Patient patient = PatService.GetWhere(PatientService.PhoneCol == username).FirstOrDefault();
                SystemAdmin admin = SysService.GetWhere(SystemAdminService.EmailCol == username).FirstOrDefault();
                if (pharmacist == null && username != null)
                {
                    List<Job> jobs = new List<Job>();
                    Pharmacist pharma = new Pharmacist() { Email = username, FirstName = username, LastName = "Not Set", PasswordHash = PPOKPrincipal.HashPassword(password), Phone = "1234567890", Fills = new List<FillHistory>() };
                    jobs.Add(new Job() { IsAdmin = true, IsActive = true, Pharmacist = pharma });
                    using (var jobService = new JobService())
                    {
                        foreach (var job in jobs)
                        {
                            //jobService.Create(job);
                        }
                    }
                    pharma.Jobs = jobs;
                    PharmService.Create(pharma);
                    pharmacist = PharmService.GetWhere(PharmacistService.EmailCol == username).FirstOrDefault();
                    //user = new User(service.GetWhere(PharmacistService.EmailCol == email).FirstOrDefault());
                }


                if (admin != null && PPOKPrincipal.IsValid(admin.Email, password))
                {
                    var serializedAdmin = new PPOKPrincipalSerializeModel(admin);
                    serializedAdmin.AddToRole("Admin");
                    makeAuthTicket(serializedAdmin);
                    return RedirectToAction("Index", "Home");
                }
                else if (pharmacist != null && PPOKPrincipal.IsValid(username, password))
                {
                    using (var service = new JobService())
                    {
                        pharmacist.Jobs = service.GetWhere(JobService.CodeCol == pharmacist.Code);
                    }
                    using (var service = new FillHistoryService())
                    {
                        pharmacist.Fills = service.GetWhere(FillHistoryService.PharmacistCodeCol == pharmacist.Code);
                    }
                    var serializedPharmacist = new PPOKPrincipalSerializeModel(pharmacist);
                    serializedPharmacist.AddToRole("Pharmacist");
                    makeAuthTicket(serializedPharmacist);
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View("Index", new LoginModel());
                }
            }

            //return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private void makeAuthTicket(PPOKPrincipalSerializeModel user)
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string userData = serializer.Serialize(user);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(3), false, userData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(authCookie);
        }
    }
}