using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public enum MessageTemplateType
    {
        PHONE, EMAIL
    }

    public class MessageTemplate
    {
        [PrimaryKey]
        public MessageTemplateType Type { get; set; }
        public string Content { get; set; }

        public MessageTemplate()
        {

        }

        public MessageTemplate(MessageTemplateType type, string content)
        {
            Type = type;
            Content = content;
        }
    }
}
