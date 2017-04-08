using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Models
{
    public class LoginModel
    {
        public List<Pharmacy> pharmacyList { get; set; }

        public LoginModel(string email)
        {
            pharmacyList = new List<Pharmacy>();
            using(var service = new PharmacistService())
            {
                var pharmacist = service.GetWhere(PharmacistService.EmailCol == email).FirstOrDefault();
                if (pharmacist != null)
                {
                    var jobs = pharmacist.Jobs;
                    foreach (var job in jobs)
                    {
                        var pharmacy = job.Pharmacy;
                        pharmacyList.Add(job.Pharmacy);
                    }

                }            }
            using (var service = new SystemAdminService())
            {
                var admin = service.GetWhere(SystemAdminService.EmailCol == email).FirstOrDefault();
                if(admin != null)
                {
                    pharmacyList.Add(new Pharmacy(-1, "System Admin", "000-000-0000", "no address"));
                }
            }
        }
    }
}
