using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class PrescriptionService : CRUDService<Prescription>
    {
        public const string TABLE = "Prescription";
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column PatientCodeCol = new Column { table = TABLE, column = "PatientCode" };
        public static readonly Column DrugCodeCol = new Column { table = TABLE, column = "DrugCode" };
        public static readonly Column SupplyCol = new Column { table = TABLE, column = "Supply" };
        public static readonly Column RefillsCol = new Column { table = TABLE, column = "Refills" };

        public PrescriptionService() : base(TABLE)
        {

        }
    }
}
