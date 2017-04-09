using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class SystemAdminService : CRUDService<SystemAdmin>
    {
        public const string TABLE = "SystemAdmin";
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column FirstNameCol = $"[{TABLE}].[FirstName]";
        public static readonly Column LastNameCol = $"[{TABLE}].[LastName]";
        public static readonly Column EmailCol = $"[{TABLE}].[Email]";
        public static readonly Column PhoneCol = $"[{TABLE}].[Phone]";

        public SystemAdminService() : base(TABLE)
        {

        }
    }
}
