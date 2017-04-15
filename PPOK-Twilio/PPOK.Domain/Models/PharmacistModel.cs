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
        public bool isActive { get; set; }
        public bool isAdmin { get; set; }
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
            int jobCount = p.Jobs.Count();
            if(jobCount == 1)
            {
                PharmacyPhone = p.Jobs.FirstOrDefault().Pharmacy.Phone;
                PharmacyAddress = p.Jobs.FirstOrDefault().Pharmacy.Address;
            }
            else
            {
                PharmacyPhone = "N/A";
                PharmacyAddress = "N/A";
            }
            //below doesnt work for some reason, i believe because it still tries to evaluate the p.jobs stuff regardless
            //PharmacyPhone = jobCount > 1 ? "N/A" : p.Jobs.FirstOrDefault().Pharmacy.Phone;
            //PharmacyAddress = jobCount > 1 ? "N/A" : p.Jobs.FirstOrDefault().Pharmacy.Address;
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
                var temp1 = p.AllJobs.Where(x => x.Code == p.Code).FirstOrDefault();
                if (temp1 != null)
                {
                    isAdmin = temp1.IsAdmin;
                    isActive = temp1.IsActive;
                }
                PharmacyName = temp.Name;
                PharmacyPhone = temp.Phone;
                PharmacyAddress = temp.Address;
            }
            this.PharmacyCode = PharmacyCode;
        }
    }
}
