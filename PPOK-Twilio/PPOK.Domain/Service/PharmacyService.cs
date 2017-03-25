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
        public const string TABLE = "Pharmacy";
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column NameCol = $"[{TABLE}].[Name]";
        public static readonly Column PhoneCol = $"[{TABLE}].[Phone]";
        public static readonly Column AddressCol = $"[{TABLE}].[Address]";

        public PharmacyService() : base(TABLE)
        {

        }
    }
}
