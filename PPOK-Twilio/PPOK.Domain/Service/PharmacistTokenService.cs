using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class PharmacistTokenService : CRUDService<PharmacistToken>
    {
        public const string TABLE = "PharmacistToken";
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column PharmacistCol = $"[{TABLE}].[PharmacistCode]";
        public static readonly Column TokenCol = $"[{TABLE}].[Token]";

        public PharmacistTokenService() : base(TABLE)
        {

        }
    }
}
