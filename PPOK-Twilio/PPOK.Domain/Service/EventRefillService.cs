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
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column EventCodeCol = $"[{TABLE}].[EventCode]";
        public static readonly Column PrescriptionCodeCol = $"[{TABLE}].[PrescriptionCode]";

        public EventRefillService() : base(TABLE)
        {

        }
    }
}
