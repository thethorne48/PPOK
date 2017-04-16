using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Service
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HideAttribute : Attribute { }


    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute { }


    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyAttribute : Attribute
    {
        public string table;

        public ForeignKeyAttribute(string table)
        {
            this.table = table;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignMultiKeyAttribute : Attribute
    {
        public string table;
        public string prefix;

        public ForeignMultiKeyAttribute(string table, string prefix = null)
        {
            this.table = table;
            this.prefix = prefix;
        }
    }
}
