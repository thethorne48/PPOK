using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class PrescriptionService : CRUDService<Drug>
    {
        public static readonly Column CodeCol = "Code";
        public static readonly Column PatientCodeCol = "PatientCode";
        public static readonly Column DrugCodeCol = "DrugCode";
        public static readonly Column SupplyCol = "Supply";
        public static readonly Column RefillsCol = "Refills";

        public PrescriptionService() : base("Drug")
        {

        }
    }
}
