using PPOK.Domain.Models;
using PPOK.Domain.Types;
using static PPOK.Domain.Utility.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PPOK.Domain.Service
{
    class EmailTemplateService
    {
        private static string relativeEmailUrl = "Email/HandleResponse?";
        private static string unsubscribeKeyword = "unsubscribe";
        
        public static string GetHTML(Event e, List<MessageResponseOption> options = null)
        {
            string templateString = "<div>{EmailBody}</div><div>{EmailButtons}</div><hr style=\"margin-top:20px; border-color:gray;\"><div style=\"color:gray;\">{Footer}</div>";
            string optionsString = GetOptionsHTML(e, options);
            string unsubscribeString = GetUnsubscribeHTML(e, options);
            string emailHtml = Utility.Util.NamedFormat(templateString, new { EmailBody = e.Message, EmailButtons = optionsString, Footer = unsubscribeString });
            return emailHtml;
        }

        private static string GetUnsubscribeHTML(Event e, List<MessageResponseOption> options)
        {
            MessageResponseOption unsubscriber = options.Find(o => o.ShortDescription != null && o.ShortDescription.ToLower().Equals(unsubscribeKeyword));
            if (unsubscriber == null)
            {
                return "";
            }
            string url = GetURL(unsubscriber, e);
            string htmlString = "Don't want to hear from " + e.Patient.Pharmacy.Name + "? | <a href=\"" + url + "\" >Unsubscribe</a>";
            return htmlString;
        }

        private static string GetOptionsHTML(Event e, List<MessageResponseOption> options)
        {
            string templateString = "<a href=\"{Url}\" style=\"background-color:#EB7035;border:1px solid #EB7035;border-radius:5px;color:#ffffff;display:inline-block;font-family:sans-serif;font-weight:bold;font-size:16px;line-height:44px;text-align:center;text-decoration:none;width:150px;margin:5px;-webkit-text-size-adjust:none;\">{Description}</a>";
            StringBuilder sb = new StringBuilder();
            foreach(MessageResponseOption opt in options)
            {
                if (!string.IsNullOrWhiteSpace(opt.ShortDescription) && !opt.ShortDescription.ToLower().Equals(unsubscribeKeyword)) {
                    string url = GetURL(opt, e);
                    string optionHtml = Utility.Util.NamedFormat(templateString, new { Url = url, Description = opt.ShortDescription.ToUpper() });
                    sb.Append(optionHtml);
                }
            }
            return sb.ToString();
        }

        private static string GetURL(MessageResponseOption opt, Event e)
        {
            return ExternalUrl + relativeEmailUrl + "optCode=" + opt.Code + "&eventCode=" + e.Code; ;
        }
    }
}
