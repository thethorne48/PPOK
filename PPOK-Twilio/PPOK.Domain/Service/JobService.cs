using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class JobService : CRUDService<Job>
    {
        public const string TABLE = "Job";
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column PharmacyCodeCol = new Column { table = TABLE, column = "PharmacyCode" };
        public static readonly Column PharmacistCodeCol = new Column { table = TABLE, column = "PharmacistCode" };
        public static readonly Column IsActiveCol = new Column { table = TABLE, column = "IsActive" };
        public static readonly Column IsAdminCol = new Column { table = TABLE, column = "IsAdmin" };

        public JobService() : base(TABLE)
        {

        }
    }
}
