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
        
        public DayEventModel(Event e)
        {
            Code = e.Code;
            FirstName = e.Patient.FirstName;
            LastName = e.Patient.LastName;
            Phone = e.Patient.Phone;

            switch (e.Type)
            {
                case Types.EventType.REFILL:
                    EventType = "Refill";
                    var data = e.Refills.First();
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
