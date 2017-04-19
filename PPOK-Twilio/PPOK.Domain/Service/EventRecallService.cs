using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    public class EventRecallService : CRUDService<EventRecall>
    {
        public const string TABLE = "EventRecall";
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column EventCodeCol = new Column { table = TABLE, column = "EventCode" };
        public static readonly Column PatientCodeCol = new Column { table = TABLE, column = "PatientCode" };

        public EventRecallService() : base(TABLE)
        {

        }

    }
}
