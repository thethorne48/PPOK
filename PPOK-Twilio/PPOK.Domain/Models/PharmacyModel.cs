using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Models
{
    public class PharmacyModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public PharmacyModel(Pharmacy p)
        {
            Code = p.Code;
            Name = p.Name;
            Phone = p.Phone;
            Address = p.Address;
         }
    }
}
