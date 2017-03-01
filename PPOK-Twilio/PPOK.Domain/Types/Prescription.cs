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
        [PrimaryKey, Identity]
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
    }
}
