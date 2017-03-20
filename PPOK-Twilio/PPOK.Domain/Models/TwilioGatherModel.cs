using System.Collections.Generic;

namespace PPOK.Domain.Models
{
    public class TwilioGatherModel : TwilioResponseModel
    {
        /// <summary>
        /// Message after user times out after not typing a key on the phone pad
        /// </summary>
        public string NoGatherMessage;
        /// <summary>
        /// Relative path to the next Url POST that will service the user after they do not type a key when asked to
        /// (a timeout happens after 5 seconds)
        /// </summary>
        public string NoGatherRedirect;

        public List<TwilioGatherOption> Options;
    }
}