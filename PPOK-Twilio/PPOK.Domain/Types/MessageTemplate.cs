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
        REFILL, RECALL, HAPPYBIRTHDAY
    }

    public enum MessageTemplateMedia
    {
        PHONE, EMAIL, NONE
    }

    public class MessageTemplate
    {
        [PrimaryKey]
        public MessageTemplateType Type { get; set; }
        [PrimaryKey]
        public MessageTemplateMedia Media { get; set; }
        public string Content { get; set; }

        public MessageTemplate()
        {

        }

        public MessageTemplate(MessageTemplateType type, MessageTemplateMedia media, string content)
        {
            Type = type;
            Media = media;
            Content = content;
        }
    }
}
