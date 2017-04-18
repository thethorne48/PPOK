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
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
		public static readonly Column ExternalIdCol = new Column { table = TABLE, column = "ExternalId" };
        public static readonly Column EventCodeCol = new Column { table = TABLE, column = "EventCode" };
        public static readonly Column StatusCol = new Column { table = TABLE, column = "Status" };
        public static readonly Column DateCol = new Column { table = TABLE, column = "Date" };

        public EventHistoryService() : base(TABLE)
        {

        }
    }
}
