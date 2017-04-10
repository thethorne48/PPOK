using PPOK.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Types
{
    public class MessageResponseOption
    {
        [PrimaryKey]
        public int Code { get; set; }
        [ForeignKey("MessageTemplate")]
        public MessageTemplate MessageTemplate { get; set; }
        public string CallbackFunction { get; set; }
        public string LongDescription { get; set; }
        public string ShortDescription { get; set; }
        public string Verb { get; set; }

        public MessageResponseOption()
        {

        }

        public MessageResponseOption(MessageTemplate template, string callbackFunc, string longDescription, string shortDescription, string verb)
        {
            MessageTemplate = template;
            CallbackFunction = callbackFunc;
            LongDescription = longDescription;
            ShortDescription = shortDescription;
            Verb = verb;
        }
    }
}
