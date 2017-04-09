using PPOK.Domain.Service;
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
            PharmacyName = "";
            foreach (var j in p.Jobs)
            {
                PharmacyName += j.Pharmacy.Name +",";
            }
            PharmacyName = PharmacyName.TrimEnd(',');
            PharmacyPhone = "N/A";
            PharmacyAddress = "N/A";
        }
        public PharmacistModel(Pharmacist p, int PharmacyCode)
        {
            Code = p.Code;
            FirstName = p.FirstName;
            LastName = p.LastName;
            Email = p.Email;
            Phone = p.Phone;
            using (var service = new PharmacyService())
            {
                var temp = service.Get(PharmacyCode);
                PharmacyName = temp.Name;
                PharmacyPhone = temp.Phone;
                PharmacyAddress = temp.Address;
            }
            this.PharmacyCode = PharmacyCode;
        }
    }
}
