using PPOK_Twilio.Auth;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PPOK.Domain.Service;
using PPOK.Domain.Types;
using PPOK.Domain.Models;
using System.Web.Script.Serialization;
using PPOK.Domain.Auth;
using PPOK.Domain.Utility;

namespace PPOK_Twilio.Controllers
{
    public class AccountController : BaseController
    {
        private const int TOKEN_LENGTH = 6;
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
        public ActionResult SendCode(int area, int prefix, int number)
        {
            string phone = area.ToString() + "-" + prefix.ToString() + "-" + number.ToString();
            var patient = AuthService.SendPatientToken(phone, PPOKPrincipal.generateRandomCode(TOKEN_LENGTH));
            if(patient != null)
            {
                makeAuthTicket(new PPOKPrincipalSerializeModel(patient));
                return View("VerifyCode");
            }
            else
            {
                ViewBag.Error = "That number was not found in our system.";
                return View("Patient");
            }
        }

        [HttpGet]
        public ActionResult VerifyCode()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyCode(string token)
        {
            var patient = AuthService.VerifyPatientToken(token);
            if(patient != null)
            {
                makeAuthTicket(new PPOKPrincipalSerializeModel(patient));
                return Redirect("/PatientMCP"); // redirect to Patient MCP
            }
            else
            {
                ViewBag.Error = "That is an invalid code";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (PPOKPrincipal.IsValid(email, password))
            {
                using (var PharmService = new PharmacistService())
                using (var SysService = new SystemAdminService())
                {
                    Pharmacist pharmacist = PharmService.GetWhere(PharmacistService.EmailCol == email).FirstOrDefault();
                    SystemAdmin admin = SysService.GetWhere(SystemAdminService.EmailCol == email).FirstOrDefault();
                    var logins = new LoginModel(email);

                    if (logins.pharmacyList.Count > 1)
                    {
                        if (admin != null)
                            makeAuthTicket(new PPOKPrincipalSerializeModel(admin));
                        else
                            makeAuthTicket(new PPOKPrincipalSerializeModel(pharmacist));
                        return View("PharmacySelect", logins);
                    }
                    else if (admin != null)
                    {
                        var serializedAdmin = new PPOKPrincipalSerializeModel(admin);
                        makeAuthTicket(serializedAdmin);
                        return RedirectToAction("Index", "SystemAdmin");
                    }
                    else if (pharmacist != null)
                    {
                        var serializedPharmacist = new PPOKPrincipalSerializeModel(pharmacist);
                        makeAuthTicket(serializedPharmacist);
                        return RedirectToAction("Index", "LandingPage");
                    }
                }
            }
            ViewBag.Error = "Invalid username/password combination";
            return View("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult PharmacySelect()
        {
            return View(new LoginModel(User.Email));
        }

        [HttpPost]
        [Authorize]
        public ActionResult PharmacySelect(int pharmacy)
        {
            User.setPharmacy(pharmacy);
            if (pharmacy > -1)
            {
                using (var service = new PharmacistService())
                {
                    var pharmacist = service.GetWhere(PharmacistService.EmailCol == User.Email).FirstOrDefault();
                    var serializedPharmacist = new PPOKPrincipalSerializeModel(pharmacist);
                    serializedPharmacist.Pharmacy = User.Pharmacy;
                    makeAuthTicket(serializedPharmacist);
                }
                return RedirectToAction("Index", "LandingPage");
            }
            else
            {
                using (var service = new SystemAdminService())
                {
                    var admin = service.GetWhere(SystemAdminService.EmailCol == User.Email).FirstOrDefault();
                    var serializedAdmin = new PPOKPrincipalSerializeModel(admin);
                    serializedAdmin.Pharmacy = User.Pharmacy;
                    makeAuthTicket(serializedAdmin);
                }
                return RedirectToAction("PharmacyView", "SystemAdmin");
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
            var pharmcist = AuthService.SendPharmacistToken(email, PPOKPrincipal.generateRandomCode(TOKEN_LENGTH));
            if (pharmcist != null)
                return View("ResetPassword");
            var sysAdmin = AuthService.SendSystemAdminToken(email, PPOKPrincipal.generateRandomCode(TOKEN_LENGTH));
            if (sysAdmin != null)
                return View("ResetPassword");
            ViewBag.Error = "That email was not found";
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string token, string password)
        {
            if (!PPOKPrincipal.passwordComplexity(password))
            {
                ViewBag.Error = "Password is not complex enough. Be sure to follow all the rules";
                return View();
            }
            bool resetAdminPass = false, resetPharmacistPass = false;
            var sysAdmin = AuthService.VerifySystemAdminToken(token);
            var pharmacist = AuthService.VerifyPharmacistToken(token);
            if(sysAdmin != null)
                resetAdminPass = AuthService.ResetSystemAdminPassword(token, sysAdmin, PPOKPrincipal.HashPassword(sysAdmin, password));
            if (pharmacist != null)
                resetPharmacistPass = AuthService.ResetPharmacistPassword(token, pharmacist, PPOKPrincipal.HashPassword(pharmacist, password));
            if (resetAdminPass || resetPharmacistPass)
                return View("Index");
            ViewBag.Error = "That token was not correct. Try again";
            return View("ForgotPassword");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return View("Index");
        }

        private void makeAuthTicket(PPOKPrincipalSerializeModel user)
        {
            FormsAuthentication.SignOut();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (user.Pharmacy != null)
            {
                // this clears any circular references
                user.getPharmacy().AllJobs = null;
                user.getPharmacy().Patients = null;
            }
            string userData = serializer.Serialize(user);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddHours(Config.TokenDuration), false, userData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(authCookie);
        }
    }
}