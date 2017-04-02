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
                        
                        PPOKPrincipal newUser = new PPOKPrincipal(authTicket.Name);
                        newUser.addRole(serializeModel.getRoles()); // todo: build user roles after login. Just store what is important here (email, code, pharmcay?)
                        newUser.Code = serializeModel.Code;
                        newUser.FirstName = serializeModel.FirstName;
                        newUser.LastName = serializeModel.LastName;
                        newUser.Email = serializeModel.Email;
                        newUser.Phone = serializeModel.Phone;
                        newUser.Pharmacy = serializeModel.Pharmacy;

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
