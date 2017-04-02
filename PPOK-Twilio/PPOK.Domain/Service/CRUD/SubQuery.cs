using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PPOK.Domain.Service
{
    public class SubQuery<T> : IEnumerable<T> where T : new()
    {
        private string table;
        private Condition condition;

        private IEnumerable<T> enumerable = null;

        public SubQuery(string table, Condition condition)
        {
            this.table = table;
            this.condition = condition;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (enumerable == null)
            {
                using (var service = new CRUDService<T>(table))
                {
                    enumerable = service.GetWhere(condition);
                }
            }
            return enumerable.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public SubQuery<T> Where(Condition condition)
        {
            return new SubQuery<T>(this.table, this.condition & condition);
        }
    }
}
