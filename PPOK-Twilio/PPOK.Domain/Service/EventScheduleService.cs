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
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column EventCodeCol = $"[{TABLE}].[EventCode]";
        public static readonly Column DateCol = $"[{TABLE}].[Date]";

        public EventScheduleService() : base(TABLE)
        {

        }

        public List<Event> GetEventsForToday()
        {
            return this.GetWhere(DateCol == DateTime.Today)
                .Select(schedule => schedule.Event)
                .ToList();
        }

        public List<Event> GetEventsBeforeToday()
        {
            return this.GetWhere(DateCol <= DateTime.Today)
                .Select(schedule => schedule.Event)
                .ToList();
        }
    }
}
