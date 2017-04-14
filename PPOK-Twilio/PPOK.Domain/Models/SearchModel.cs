using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Models
{
    public class SearchModel
    {
        public int Code { get; set; }
        public string EventType { get; set; }
        public string Name { get; set; }
        public string PrescriptionName { get; set; }
        public string PrescriptionNumber { get; set; }
        public string Phone { get; set; }
        public string LastActivity { get; set; }
        public string Status { get; set; } //probably make this an enum thing
        
        public SearchModel(Event e)
        {
            Code = e.Code;
            Name = e.Patient.Name;
            Phone = e.Patient.Phone;
            Status = e.Status.ToString();

            var last = e.History.OrderBy(x => x.Date).FirstOrDefault();
            LastActivity = last != null ? last.Date.ToShortDateString() : "N/A";

            switch (e.Type)
            {
                case Types.EventType.REFILL:
                    EventType = Constants.constRefillEvent;
                    //var data = e.Refills.First();
                    //PrescriptionName = data.Prescription.Drug.Name;
                    break;
                case Types.EventType.BIRTHDAY:
                    EventType = Constants.constBirthdayEvent;
                    PrescriptionName = Constants.constNotApplicable;
                    break;
                case Types.EventType.RECALL:
                    EventType = Constants.constRecallEvent;
                    PrescriptionName = Constants.constNotApplicable;
                    break;
            }
        }
    }
}
