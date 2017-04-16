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
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column EventRefillCodeCol = new Column { table = TABLE, column = "EventRefillCode" };
        public static readonly Column PharmacistCodeCol = new Column { table = TABLE, column = "PharmacistCode" };
        public static readonly Column DateCol = new Column { table = TABLE, column = "Date" };

        public FillHistoryService() : base(TABLE)
        {

        }
    }
}
