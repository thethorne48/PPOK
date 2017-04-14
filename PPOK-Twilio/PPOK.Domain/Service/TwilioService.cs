using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using static PPOK.Domain.Utility.Config;

namespace PPOK.Domain.Service
{
    /// <summary>
    /// Services API requests and responses with Twilio. Requires a localtunnel to be open with a base url
    /// equal to the ExternalUrl attribute for this class. For a trial account, your phone number 
    /// must be verified in Twilio to communicate with this API.
    /// </summary>
    public class TwilioService
    {
        public static string GetId(CallResource call)
        {
            return call.ParentCallSid;
        }

        public static string GetId(MessageResource text)
        {
            return text.Sid;
        }

        /// <summary>
        /// Send a SMS message to the specified phone number.
        /// You can specify instructions on how to respond separately from the main message body by including message options.
        /// </summary>
        /// <param name="toNumber">phone number to send the SMS to. Standard rates apply.</param>
        /// <param name="messageBody">contents of the SMS message</param>
        /// <param name="options">how the user is expected to reply</param>
        /// <returns></returns>
        public static MessageResource SendSMSMessage(string toNumber, string messageBody, List<MessageResponseOption> options = null)
        {
            TwilioClient.Init(TwilioAccountSid, TwilioAuthToken);

            string optionsStr = (options == null) ? "" : GetTextOptions(options);

            var message = MessageResource.Create(
                 to: new PhoneNumber(toNumber),
                 messagingServiceSid: TwilioMessageServiceSid,
                 body: messageBody + optionsStr);

            if (message.ErrorCode != null)
            {
                throw new Exception("Twilio encountered an error: " + message.ErrorCode + " - " + message.ErrorMessage);
            }

            return message;
        }
        /// <summary>
        /// Calls the specified phone number with the starting TwiML message script found at the relative url
        /// </summary>
        /// <param name="toNumber">phone number to open a phone call to. Standard rates apply.</param>
        /// <param name="relativeUrl">Relative url to the page that generates the TwiML for this message's contents, i.e. "/MyController/TwilioCall?param=3"</param>
        public static CallResource SendVoiceMessage(string toNumber, string relativeUrl)
        {
            TwilioClient.Init(TwilioAccountSid, TwilioAuthToken);
            
            IncomingPhoneNumberResource phoneResource = IncomingPhoneNumberResource.Fetch(TwilioPhoneSid);
            CallResource call = CallResource.Create(to: new PhoneNumber(toNumber),
                                           from: phoneResource.PhoneNumber,
                                           url: new Uri(ExternalUrl + relativeUrl));
            return call;
        }

        private static string GetTextOptions(List<MessageResponseOption> options)
        {
            options = options.FindAll(o => { return o.Verb != null && o.ShortDescription != null; });
            if (options.Count == 0)
            {
                return "";
            }
            
            StringBuilder sb = new StringBuilder(" Reply with ");
                
            for (int i = 0; i < options.Count; i++)
            {
                MessageResponseOption opt = options[i];
                sb.Append(opt.Verb.ToUpper());
                sb.Append(" to ");
                sb.Append(opt.ShortDescription);
                if (i + 1 < options.Count)
                {
                    sb.Append(", ");
                }
                else
                {
                    sb.Append(".");
                }
            }
            return sb.ToString();
        }
    }
}

