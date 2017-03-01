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
        public static readonly Column CodeCol = "Code";
        public static readonly Column PrescriptionCodeCol = "PrescriptionCode";
        public static readonly Column TypeCol = "Type";
        public static readonly Column MessageCol = "Message";

        public EventService() : base("Event")
        {

        }
    }
}
