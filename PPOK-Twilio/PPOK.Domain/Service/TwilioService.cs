using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace PPOK.Domain.Service
{
    public class TwilioService
    {
        private static readonly string AccountSid = "ACdc4baa0b5fceb40de713632d9ed04e7d";
        private static readonly string AuthToken = "2dda1785d4239e9cb0bb8c251f3ff0d9";
        private static readonly string MessageServiceSid = "MG546d4d7950cc9497c1b5dbdd89fe9829";

        public static MessageResource SendSMSMessage(string toNumber, string messageBody)
        {
            TwilioClient.Init(AccountSid, AuthToken);

            var message = MessageResource.Create(
                 to: new PhoneNumber(toNumber),
                 messagingServiceSid: MessageServiceSid,
                 body: messageBody);

            if (message.ErrorCode != null)
            {
                throw new Exception("Twilio encountered an error: " + message.ErrorCode + " - " + message.ErrorMessage);
            }
            if (message.DateSent == null)
            {
                throw new Exception("Twilio did not send the message, see Twilio's console log for details.");
            }

            return message;
        }
    }
}

