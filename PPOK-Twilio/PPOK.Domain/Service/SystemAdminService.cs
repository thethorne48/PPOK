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
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column FirstNameCol = new Column { table = TABLE, column = "FirstName" };
        public static readonly Column LastNameCol = new Column { table = TABLE, column = "LastName" };
        public static readonly Column EmailCol = new Column { table = TABLE, column = "Email" };
        public static readonly Column PhoneCol = new Column { table = TABLE, column = "Phone" };

        public SystemAdminService() : base(TABLE)
        {

        }
    }
}
