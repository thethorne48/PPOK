using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    public interface IDatabaseInterface : IDisposable
    {
        void Execute(string sql, object parameters=null);

        IEnumerable<dynamic> Query(string sql, object parameters=null);
    }
}
