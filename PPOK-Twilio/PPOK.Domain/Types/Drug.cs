using System.Collections.Generic;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class Drug
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        public string Name { get; set; }
        [ForeignMultiKey("Prescription")]
        public IEnumerable<Prescription> Prescriptions { get; set; }
    }
}
