using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class DrugService : CRUDService<Drug>
    {
        public const string TABLE = "Drug";
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column NameCol = $"[{TABLE}].[Name]";

        public DrugService() : base(TABLE)
        {

        }
    }
}
