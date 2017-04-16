using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class EventService : CRUDService<Event>
    {
        public const string TABLE = "Event";
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column StatusCol = new Column { table = TABLE, column = "Status" };
        public static readonly Column TypeCol = new Column { table = TABLE, column = "Type" };
        public static readonly Column MessageCol = new Column { table = TABLE, column = "Message" };

        public EventService() : base(TABLE)
        {

        }
    }
}
