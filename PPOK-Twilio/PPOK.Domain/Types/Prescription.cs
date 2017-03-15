using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class Prescription
    {
        [PrimaryKey]
        public int Code { get; set; }
        [ForeignKey("Patient")]
        public Patient Patient { get; set; }
        [ForeignKey("Drug")]
        public Drug Drug { get; set; }
        public int Supply { get; set; }
        public int Refills { get; set; }
        [ForeignMultiKey("Event")]
        public IEnumerable<Event> Events { get; set; }
        [ForeignMultiKey("FillHistory")]
        public IEnumerable<FillHistory> Fills { get; set; }

        public Prescription()
        {

        }

        public Prescription(int code, Patient patient, Drug drug, int supply, int refills)
        {
            Code = code;
            Patient = patient;
            Drug = drug;
            Supply = supply;
            Refills = refills;
        }
    }

}
