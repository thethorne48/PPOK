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

        public SearchModel(EventRefill e)
        {
            Code = e.Event.Code;
            EventType = "Refill Event";
            Name = e.Prescription.Patient.Name;
            PrescriptionName = e.Prescription.Drug.Name;
            PrescriptionNumber = e.Prescription.Code.ToString();
            Phone = e.Prescription.Patient.Phone;
            LastActivity = e.Event.History.OrderBy(x=>x.Date).FirstOrDefault() != null ? e.Event.History.OrderBy(x => x.Date).FirstOrDefault().Date.ToShortDateString() : "N/A";
            Status = e.Event.Status.ToString();
        }
        public SearchModel(EventBirthday e)
        {
            Code = e.Event.Code;
            EventType = "Birthday Event";
            Name = e.Patient.Name;
            PrescriptionName = "N/A";
            Phone = e.Patient.Phone;
            LastActivity = e.Event.History.OrderBy(x => x.Date).FirstOrDefault() != null ? e.Event.History.OrderBy(x => x.Date).FirstOrDefault().Date.ToShortDateString() : "N/A";
            Status = e.Event.Status.ToString();
        }
        public SearchModel(EventRecall e)
        {
            Code = e.Event.Code;
            EventType = "Recall Event";
            Name = e.Patient.Name;
            PrescriptionName = "N/A";
            Phone = e.Patient.Phone;
            LastActivity = e.Event.History.OrderBy(x=>x.Date).FirstOrDefault() != null ? e.Event.History.OrderBy(x => x.Date).FirstOrDefault().Date.ToShortDateString() : "N/A";
            Status = e.Event.Status.ToString();
        }
    }
}
