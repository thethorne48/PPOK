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
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column PatientCodeCol = $"[{TABLE}].[PatientCode]";
        public static readonly Column DrugCodeCol = $"[{TABLE}].[DrugCode]";
        public static readonly Column SupplyCol = $"[{TABLE}].[Supply]";
        public static readonly Column RefillsCol = $"[{TABLE}].[Refills]";

        public PrescriptionService() : base(TABLE)
        {

        }
    }
}
