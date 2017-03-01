using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class Job
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("Pharmacy")]
        public Pharmacy Pharmacy { get; set; }
        [ForeignKey("Pharmacist")]
        public Pharmacist Pharmacist { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
    }
}
