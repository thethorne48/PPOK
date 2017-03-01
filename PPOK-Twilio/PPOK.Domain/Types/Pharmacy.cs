using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class Pharmacy
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [ForeignMultiKey("Job")]
        public IEnumerable<Job> Jobs { get; set; }
        [ForeignMultiKey("Patient")]
        public IEnumerable<Patient> Patients { get; set; }
    }
}
