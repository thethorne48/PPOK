using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Models
{
    public class PharmacistModel
    {
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyPhone { get; set; }
        public int PharmacyCode { get; set; }
        public string PharmacyAddress { get; set; } 

        public PharmacistModel(Pharmacist p)
        {
            Code = p.Code;
            FirstName = p.FirstName;
            LastName = p.LastName;
            Email = p.Email;
            Phone = p.Phone;
            PharmacyName = "nope";
            PharmacyPhone = "8675309";
            PharmacyAddress = "taco";
        }
        public PharmacistModel(Pharmacist p, int PharmacyCode)
        {
            Code = p.Code;
            FirstName = p.FirstName;
            LastName = p.LastName;
            Email = p.Email;
            Phone = p.Phone;
            PharmacyName = "nope";
            PharmacyPhone = "8675309";
            PharmacyAddress = "taco";
            this.PharmacyCode = PharmacyCode;
        }
    }
}
