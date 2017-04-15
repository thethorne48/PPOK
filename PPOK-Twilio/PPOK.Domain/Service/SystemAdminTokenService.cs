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
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column SystemAdminCodeCol = $"[{TABLE}].[SystemAdminCode]";
        public static readonly Column TokenCol = $"[{TABLE}].[Token]";

        public SystemAdminTokenService() : base(TABLE)
        {

        }
    }
}
