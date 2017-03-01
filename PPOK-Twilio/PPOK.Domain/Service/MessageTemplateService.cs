using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class MessageTemplateService : CRUDService<MessageTemplate>
    {
        public static readonly Column CodeCol = "Code";
        public static readonly Column NameCol = "Name";
        public static readonly Column ContentCol = "Content";

        public MessageTemplateService() : base("MessageTemplate")
        {

        }
    }
}
