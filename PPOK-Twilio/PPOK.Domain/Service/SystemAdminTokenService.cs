using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class SystemAdminTokenService : CRUDService<SystemAdminToken>
    {
        public const string TABLE = "SystemAdminToken";
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column SystemAdminCodeCol = new Column { table = TABLE, column = "SystemAdminCode" };
        public static readonly Column TokenCol = new Column { table = TABLE, column = "Token" };

        public SystemAdminTokenService() : base(TABLE)
        {

        }
    }
}
