using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class PharmacistService : CRUDService<Pharmacist>
    {
        public static readonly Column CodeCol = "Code";
        public static readonly Column FirstNameCol = "FirstName";
        public static readonly Column LastNameCol = "LastName";
        public static readonly Column EmailCol = "Email";
        public static readonly Column PhoneCol = "Phone";

        public PharmacistService() : base("Pharmacist")
        {

        }
    }
}
