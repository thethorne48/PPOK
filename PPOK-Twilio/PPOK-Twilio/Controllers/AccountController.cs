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
        public ActionResult SendCode(int area, int prefix, int number)
        {
            string phone = area.ToString() + "-" + prefix.ToString() + "-" + number.ToString();
            using (var service = new PatientService())
            {
                var patient = service.GetWhere(PatientService.PhoneCol == phone).FirstOrDefault();
                if (patient != null)
                {
                    string token;
                    using (var tokenService = new PatientTokenService())
                    {
                        PatientToken populatedToken;
                        do
                        {
                            token = PPOKPrincipal.generateRandomCode(6);
                            populatedToken = tokenService.GetWhere(PatientTokenService.TokenCol == token).FirstOrDefault();
                        } while (populatedToken != null);

                        var storedToken = tokenService.GetWhere(PatientTokenService.PatientCodeCol == patient.Code).FirstOrDefault();
                        if (storedToken == null)
                            tokenService.Create(new PatientToken(patient, token));
                        else
                        {
                            storedToken.Token = token;
                            tokenService.Update(storedToken);
                        }
                    }
                    TwilioService.SendSMSMessage(patient.Phone, "Please enter this code to login: " + token);
                    PPOKPrincipalSerializeModel serializedPatient = new PPOKPrincipalSerializeModel(patient);
                    serializedPatient.Type = AccountTypes.Patient;
                    makeAuthTicket(serializedPatient);
                    return View("VerifyCode");
                }
                else
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
            using (var service = new PatientTokenService())
            {
                var patientToken = service.GetWhere(PatientTokenService.TokenCol == token).FirstOrDefault();
                if(patientToken != null)
                {
                    PPOKPrincipalSerializeModel serializedPatient = new PPOKPrincipalSerializeModel(patientToken.Patient);
                    serializedPatient.Type = AccountTypes.Patient;
                    makeAuthTicket(serializedPatient);
                    //service.Delete(patientToken);
                    return Redirect("/PatientMCP"); // redirect to Patient MCP
                }
                else
                {
                    ViewBag.Error = "That is an invalid code";
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (ReturnUrl.ToLower().Contains("patient"))
                return View("Patient");
            if (ReturnUrl.Length > 3)
                return Redirect(ReturnUrl);
            return View("Index");
        }

        [HttpPost]
        public ActionResult Login(string username, string password, string ReturnUrl = "")
        {
            using (var PharmService = new PharmacistService())
            using (var SysService = new SystemAdminService())
            {
                Pharmacist pharmacist = PharmService.GetWhere(PharmacistService.EmailCol == username).FirstOrDefault();
                SystemAdmin admin = SysService.GetWhere(SystemAdminService.EmailCol == username).FirstOrDefault();
                var logins = new LoginModel(username);
                if (PPOKPrincipal.IsValid(username, password))
                {
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
                        if (ReturnUrl.Length > 3)
                            return Redirect(ReturnUrl);
                        return RedirectToAction("PharmacyView", "SystemAdmin");
                    }
                    else if (pharmacist != null)
                    {
                        var serializedPharmacist = new PPOKPrincipalSerializeModel(pharmacist);
                        makeAuthTicket(serializedPharmacist);
                        if (ReturnUrl.Length > 3)
                            return Redirect(ReturnUrl);
                        return RedirectToAction("Index", "LandingPage");
                    }
                }

                ViewBag.Error = "Invalid username/password combination";
                return View("Index", new { ReturnUrl });
            }
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
            } else
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
            using (var service = new PharmacistService())
            {
                var pharmacist = service.GetWhere(PharmacistService.EmailCol == email).FirstOrDefault();
                if (pharmacist != null)
                {
                    var token = PPOKPrincipal.generateRandomCode(6);
                    TwilioService.SendSMSMessage(pharmacist.Phone, "Enter this code to reset your password: " + token);
                    using (var pharmacistTokenService = new PharmacistTokenService())
                    {
                        pharmacistTokenService.Create(new PharmacistToken(pharmacist, token));
                    }
                    return View("ResetPassword");
                }
            }
            using (var service = new SystemAdminService())
            {
                var admin = service.GetWhere(SystemAdminService.EmailCol == email).FirstOrDefault();
                if(admin != null)
                {
                    var token = PPOKPrincipal.generateRandomCode(6);
                    TwilioService.SendSMSMessage(admin.Phone, "Enter this code to reset your password: " + token);
                    using (var systemTokenService = new SystemAdminTokenService())
                    {
                        systemTokenService.Create(new SystemAdminToken(admin, token));
                    }
                    return View("ResetPassword");
                }
            }
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
            using (var systemTokenService = new SystemAdminTokenService()) // system admins can't change their password.
            using (var pharmacistTokenService = new PharmacistTokenService())
            {
                var systemToken = systemTokenService.GetWhere(SystemAdminTokenService.TokenCol == token).FirstOrDefault();
                var pharmacistToken = pharmacistTokenService.GetWhere(PharmacistTokenService.TokenCol == token).FirstOrDefault();
                if (pharmacistToken != null)
                {
                    var pharmacist = pharmacistToken.Pharmacist;
                    pharmacist.PasswordHash = PPOKPrincipal.HashPassword(pharmacist, password);
                    using (var pharmacistService = new PharmacistService())
                    {
                        pharmacistService.Update(pharmacist);
                        using (var adminService = new SystemAdminService())
                        {
                            var systemAdmin = adminService.GetWhere(SystemAdminService.EmailCol == pharmacist.Email).FirstOrDefault();
                            if (systemAdmin != null)
                            {
                                systemAdmin.PasswordHash = PPOKPrincipal.HashPassword(systemAdmin, password);
                                adminService.Update(systemAdmin);
                            }
                        }

                    }
                }
                else if (systemToken != null)
                {
                    var systemAdmin = systemToken.SystemAdmin;
                    using (var adminService = new SystemAdminService())
                    {
                        systemAdmin.PasswordHash = PPOKPrincipal.HashPassword(systemAdmin, password);
                        adminService.Update(systemAdmin);
                    }
                }
                else
                {
                    ViewBag.Error = "That token was not correct. Try again";
                    return View("ForgotPassword");
                }
            }
            return View("Index");
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
            return Redirect("/");
        }

        private void makeAuthTicket(PPOKPrincipalSerializeModel user)
        {
            FormsAuthentication.SignOut();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (user.Pharmacy != null)
            {
                user.getPharmacy().Jobs = null;
                user.getPharmacy().Patients = null;
            }
            string userData = serializer.Serialize(user);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(30), false, userData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(authCookie);
        }
    }
}