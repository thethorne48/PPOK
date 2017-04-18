using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class EventHistory
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        public string ExternalId { get; set; }
        [ForeignKey("Event")]
        public Event Event { get; set; }
        public EventStatus Status { get; set; }
        public DateTime Date { get; set; }

        public EventHistory()
        {

        }

        public EventHistory(Event even, EventStatus status, DateTime date)
        {
            Event = even;
            Status = status;
            Date = date;
        }
    }
}
