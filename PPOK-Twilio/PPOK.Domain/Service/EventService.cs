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
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column PrescriptionCodeCol = $"[{TABLE}].[PrescriptionCode]";
        public static readonly Column TypeCol = $"[{TABLE}].[Type]";
        public static readonly Column MessageCol = $"[{TABLE}].[Message]";

        public EventService() : base(TABLE)
        {

        }
    }
}
