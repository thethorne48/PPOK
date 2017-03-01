using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class PharmacyService : CRUDService<Pharmacy>
    {
        public static readonly Column CodeCol = "Code";
        public static readonly Column NameCol = "Name";
        public static readonly Column PhoneCol = "Phone";
        public static readonly Column AddressCol = "Address";

        public PharmacyService() : base("Pharmacy")
        {

        }
    }
}
