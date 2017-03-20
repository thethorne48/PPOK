namespace PPOK.Domain.Models
{
    public class TwilioResponseModel
    {
        /// <summary>
        /// Message before the prompt
        /// </summary>
        public string MessageBody;
        /// <summary>
        /// Relative path to the next Url POST that will service the user after they type a key on the phone pad
        /// </summary>
        public string Redirect;
    }
}