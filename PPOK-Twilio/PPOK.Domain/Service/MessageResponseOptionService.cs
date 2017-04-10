using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    class MessageResponseOptionService : CRUDService<MessageResponseOption>
    {
        public const string TABLE = "MessageResponseOption";
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column MessageTemplateCodeCol = $"[{TABLE}].[MessageTemplateCode]";
        public static readonly Column CallbackFunctionCol = $"[{TABLE}].[CallbackFunction]";
        public static readonly Column LongDescriptionCol = $"[{TABLE}].[LongDescription]";
        public static readonly Column ShortDescriptionCol = $"[{TABLE}].[ShortDescription]";
        public static readonly Column VerbCol = $"[{TABLE}].[Verb]";

        public MessageResponseOptionService() : base(TABLE)
        {

        }
    }
}
