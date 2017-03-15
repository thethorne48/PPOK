using System.Collections.Generic;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class Drug
    {
        [PrimaryKey]
        public long Code { get; set; }
        public string Name { get; set; }
        [ForeignMultiKey("Prescription")]
        public IEnumerable<Prescription> Prescriptions { get; set; }

        public Drug()
        {

        }

        public Drug(long code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
