using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class PatientTokenService : CRUDService<PatientToken>
    {
        public const string TABLE = "PatientToken";
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column PatientCodeCol = $"[{TABLE}].[PatientCode]";
        public static readonly Column TokenCol = $"[{TABLE}].[Token]";

        public PatientTokenService() : base(TABLE)
        {

        }
    }
}
