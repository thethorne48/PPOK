using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class FillHistoryService : CRUDService<FillHistory>
    {
        public static readonly Column CodeCol = "Code";
        public static readonly Column PrescriptionCodeCol = "PrescriptionCode";
        public static readonly Column PharmacistCodeCol = "PharmacistCode";
        public static readonly Column DateCol = "Date";

        public FillHistoryService() : base("FillHistory")
        {

        }
    }
}
