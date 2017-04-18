using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    public class MessageResponseOptionService : CRUDService<MessageResponseOption>
    {
        public const string TABLE = "MessageResponseOption";
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column MessageTemplateTypeCol = new Column { table = TABLE, column = "Type" };
        public static readonly Column CallbackFunctionCol = new Column { table = TABLE, column = "CallbackFunction" };
        public static readonly Column LongDescriptionCol = new Column { table = TABLE, column = "LongDescription" };
        public static readonly Column ShortDescriptionCol = new Column { table = TABLE, column = "ShortDescription" };
        public static readonly Column VerbCol = new Column { table = TABLE, column = "Verb" };
        

    public MessageResponseOptionService() : base(TABLE)
        {

        }
    }
}
