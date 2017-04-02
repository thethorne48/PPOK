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
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column EventCodeCol = $"[{TABLE}].[EventCode]";
        public static readonly Column PatientCodeCol = $"[{TABLE}].[PatientCode]";
        public static readonly Column DrugCodeCol = $"[{TABLE}].[DrugCode]";

        public EventRecallService() : base(TABLE)
        {

        }

    }
}
