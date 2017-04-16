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
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column PatientCodeCol = new Column { table = TABLE, column = "PatientCode" };
        public static readonly Column TokenCol = new Column { table = TABLE, column = "Token" };

        public PatientTokenService() : base(TABLE)
        {

        }
    }
}
