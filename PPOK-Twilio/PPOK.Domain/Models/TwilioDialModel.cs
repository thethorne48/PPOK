using Twilio.Types;

namespace PPOK.Domain.Models
{
    public class TwilioDialModel : TwilioResponseModel
    {
        /// <summary>
        /// Phone number to bridge the user to in the middle of the call
        /// </summary>
        public PhoneNumber DialTo;
    }
}