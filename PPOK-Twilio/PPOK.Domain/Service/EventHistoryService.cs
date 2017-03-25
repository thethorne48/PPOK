using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class EventHistoryService : CRUDService<EventHistory>
    {
        public const string TABLE = "EventHistory";
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column EventCodeCol = $"[{TABLE}].[EventCode]";
        public static readonly Column StatusCol = $"[{TABLE}].[Status]";
        public static readonly Column DateCol = $"[{TABLE}].[Date]";

        public EventHistoryService() : base(TABLE)
        {

        }
    }
}
