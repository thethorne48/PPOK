using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class EventSchedule
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("Event")]
        public Event Event { get; set; }
        public DateTime Date { get; set; }

        public EventSchedule() { }

        public EventSchedule(Event even, DateTime date)
        {
            Event = even;
            Date = date;
        }
    }
}
