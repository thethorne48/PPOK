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
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column PharmacyCodeCol = $"[{TABLE}].[PharmacyCode]";
        public static readonly Column PharmacistCodeCol = $"[{TABLE}].[PharmacistCode]";
        public static readonly Column IsActiveCol = $"[{TABLE}].[IsActive]";
        public static readonly Column IsAdminCol = $"[{TABLE}].[IsAdmin]";

        public JobService() : base(TABLE)
        {

        }
    }
}
