using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Models
{
    class LoginModel
    {
        public List<Pharmacy> pharmacyList { get; set; }

        public LoginModel()
        {
            using(var service = new PharmacyService())
            {
                pharmacyList = service.GetAll();
            }
        }
    }
}
