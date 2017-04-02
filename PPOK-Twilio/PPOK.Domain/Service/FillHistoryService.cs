using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class FillHistoryService : CRUDService<FillHistory>
    {
        public const string TABLE = "FillHistory";
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column EventRefillCodeCol = $"[{TABLE}].[EventRefillCode]";
        public static readonly Column PharmacistCodeCol = $"[{TABLE}].[PharmacistCode]";
        public static readonly Column DateCol = $"[{TABLE}].[Date]";

        public FillHistoryService() : base(TABLE)
        {

        }
    }
}
