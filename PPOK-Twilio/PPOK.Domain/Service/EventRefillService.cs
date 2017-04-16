using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    public class EventRefillService : CRUDService<EventRefill>
    {
        public const string TABLE = "EventRefill";
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column EventCodeCol = new Column { table = TABLE, column = "EventCode" };
        public static readonly Column PrescriptionCodeCol = new Column { table = TABLE, column = "PrescriptionCode" };

        public EventRefillService() : base(TABLE)
        {

        }
    }
}
