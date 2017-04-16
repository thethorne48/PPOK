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
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column PharmacyCodeCol = $"[{TABLE}].[PharmacyCode]";
        public static readonly Column TypeCol = $"[{TABLE}].[Type]";
        public static readonly Column MediaCol = $"[{TABLE}].[Media]";
        public static readonly Column ContentCol = $"[{TABLE}].[Content]";

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
