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
        public DayEventModel(EventRefill e)
        {
            Code = e.Code;
            EventType = "Refill";
            FirstName = e.Prescription.Patient.FirstName;
            LastName = e.Prescription.Patient.LastName;
            Drug = e.Prescription.Drug.Name;
            Phone = e.Prescription.Patient.Phone;
        }
        public DayEventModel(EventBirthday e)
        {
            Code = e.Code;
            EventType = "Birthday!";
            FirstName = e.Patient.FirstName;
            LastName = e.Patient.LastName;
            Drug = "N/A";
            Phone = e.Patient.Phone;
        }

    }
}
