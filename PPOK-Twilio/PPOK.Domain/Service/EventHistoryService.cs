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
        public static readonly Column CodeCol = "Code";
        public static readonly Column EventCodeCol = "EventCode";
        public static readonly Column StatusCol = "Status";
        public static readonly Column DateCol = "Date";

        public EventHistoryService() : base("EventHistory")
        {

        }
    }
}
