using PPOK_Twilio.Auth;
using System.Web.Mvc;

namespace PPOK_Twilio.Views.Base
{
    public abstract class BaseViewPage : WebViewPage
    {
        public new PPOKPrincipal User
        {
            get { return base.User as PPOKPrincipal; }
        }
    }
    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public new PPOKPrincipal User
        {
            get { return base.User as PPOKPrincipal; }
        }
    }
}