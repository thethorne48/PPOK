using PPOK.Domain.Service;
using PPOK_Twilio.Auth;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace PPOK_Twilio
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //get the username              
                        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        PPOKPrincipalSerializeModel serializeModel = serializer.Deserialize<PPOKPrincipalSerializeModel>(authTicket.UserData);
                        
                        PPOKPrincipal newUser = new PPOKPrincipal(serializeModel.Email);
                        switch (serializeModel.Type)
                        {
                            case AccountTypes.Pharmacist:
                            case AccountTypes.Admin:
                                using (var service = new PharmacistService())
                                {
                                    newUser = new PPOKPrincipal(service.Get(serializeModel.Code), serializeModel.Pharmacy.Code);
                                }
                                break;
                            case AccountTypes.Patient:
                                using (var service = new PatientService())
                                {
                                    newUser = new PPOKPrincipal(service.Get(serializeModel.Code));
                                }
                                break;
                            case AccountTypes.System:
                                using (var service = new SystemAdminService())
                                {
                                    newUser = new PPOKPrincipal(service.Get(serializeModel.Code));
                                }
                                break;
                        }

                        HttpContext.Current.User = newUser;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //somehting went wrong
                    }
                }
            }
        }
    }
}
