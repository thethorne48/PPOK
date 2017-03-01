using System.Collections;
using System.Collections.Generic;

namespace PPOK.Domain.Service
{
    public class SubQuery<T> : IEnumerable<T> where T : new()
    {
        public string table;
        public string condition;
        public object args;

        public IEnumerable<T> enumerable = null;

        public SubQuery(string table, string condition, object args)
        {
            this.table = table;
            this.condition = condition;
            this.args = args;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (enumerable == null)
            {
                using (var service = new CRUDService<T>(table))
                {
                    enumerable = service.GetWhere(condition, args);
                }
            }
            return enumerable.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
