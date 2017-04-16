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
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column PharmacistCodeCol = new Column { table = TABLE, column = "PharmacistCode" };
        public static readonly Column TokenCol = new Column { table = TABLE, column = "Token" };

        public PharmacistTokenService() : base(TABLE)
        {

        }
    }
}
