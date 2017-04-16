using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    public class EventScheduleService : CRUDService<EventSchedule>
    {
        public const string TABLE = "EventSchedule";
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column EventCodeCol = new Column { table = TABLE, column = "EventCode" };
        public static readonly Column DateCol = new Column { table = TABLE, column = "Date" };

        public EventScheduleService() : base(TABLE)
        {

        }

        public List<Event> GetEventsForToday()
        {
            return this.GetWhere(DateCol == DateTime.Today)
                .Select(schedule => schedule.Event)
                .ToList();
        }

        public List<EventSchedule> GetEventsBeforeToday()
        {
            return this.GetWhere(DateCol <= DateTime.Today);
        }
    }
}
