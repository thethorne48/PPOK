using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class Event
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("Prescription")]
        public Prescription Prescription { get; set; }
        public EventType Type { get; set; }
        public string Message { get; set; }
        [ForeignMultiKey("EventHistory")]
        public IEnumerable<EventHistory> History { get; set; }
    }
}
