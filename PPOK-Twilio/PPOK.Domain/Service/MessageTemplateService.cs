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
        public const string TABLE = "MessageTemplate";
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column PharmacyCodeCol = new Column { table = TABLE, column = "PharmacyCode" };
        public static readonly Column TypeCol = new Column { table = TABLE, column = "Type" };
        public static readonly Column MediaCol = new Column { table = TABLE, column = "Media" };
        public static readonly Column ContentCol = new Column { table = TABLE, column = "Content" };

        public MessageTemplateService() : base(TABLE)
        {

        }

        public MessageTemplate Get(Pharmacy pharmacy, MessageTemplateType type, MessageTemplateMedia media)
        {
            return GetWhere(PharmacyCodeCol == pharmacy.Code & TypeCol == type & MediaCol == media).FirstOrDefault();
        }

        public List<MessageTemplate> GetAll(Pharmacy pharmacy)
        {
            return GetWhere(PharmacyCodeCol == pharmacy.Code);
        }
    }
}
