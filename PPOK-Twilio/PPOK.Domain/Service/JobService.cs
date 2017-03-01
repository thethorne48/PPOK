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
        public static readonly Column CodeCol = "Code";
        public static readonly Column PharmacyCodeCol = "PharmacyCode";
        public static readonly Column PharmacistCodeCol = "PharmacistCode";
        public static readonly Column IsActiveCol = "IsActive";
        public static readonly Column IsAdminCol = "IsAdmin";

        public JobService() : base("Job")
        {

        }
    }
}
