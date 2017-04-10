using System;
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
        /// Send a SMS message to the specified phone number
        /// </summary>
        /// <param name="toNumber">phone number to send the SMS to. Standard rates apply.</param>
        /// <param name="messageBody">contents of the SMS message</param>
        /// <returns></returns>
        public static MessageResource SendSMSMessage(string toNumber, string messageBody)
        {
            TwilioClient.Init(TwilioAccountSid, TwilioAuthToken);

            var message = MessageResource.Create(
                 to: new PhoneNumber(toNumber),
                 messagingServiceSid: TwilioMessageServiceSid,
                 body: messageBody);

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
    }
}

