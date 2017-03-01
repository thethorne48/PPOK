using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class Patient
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("Pharmacy")]
        public Pharmacy Pharmacy { get; set; }
        public ContactPreference ContactPreference { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DOB { get; set; }
        public int ZipCode { get; set; }
        [ForeignMultiKey("Prescription")]
        public IEnumerable<Prescription> Prescriptions { get; set; }
    }
}
