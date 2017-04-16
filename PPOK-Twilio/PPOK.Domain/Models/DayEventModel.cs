using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Models
{
    public class DayEventModel
    {
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string LastName { get; set; }
        public string Drug { get; set; }
        public string EventType { get; set; }
        public int EventScheduledCode { get; set; }
        
        public DayEventModel(EventSchedule e)
        {
            EventScheduledCode = e.Code;
            Code = e.Event.Code;
            FirstName = e.Event.Patient.FirstName;
            LastName = e.Event.Patient.LastName;
            Phone = e.Event.Patient.Phone;

            switch (e.Event.Type)
            {
                case Types.EventType.REFILL:
                    EventType = "Refill";
                    var data = e.Event.Refills.First();
                    Drug = data.Prescription.Drug.Name;
                    break;
                case Types.EventType.BIRTHDAY:
                    EventType = "Birthday!";
                    Drug = "N/A";
                    break;
            }
        }
    }
}
