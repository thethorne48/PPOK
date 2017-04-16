namespace PPOK.Domain.Service
{
    public class Column
    {
        internal string table;
        internal string column;

        public Column In(string prefix)
        {
            return new Column { table = prefix + table, column = column };
        }

        public override string ToString()
        {
            return $"[{table}].[{column}]";
        }

        public static Condition operator ==(Column col, object value)
        {
            return Condition.Compare(col, "=", value);
        }

        public static Condition operator !=(Column col, object value)
        {
            return Condition.Compare(col, "!=", value);
        }

        public static Condition operator >(Column col, object value)
        {
            return Condition.Compare(col, ">", value);
        }

        public static Condition operator >=(Column col, object value)
        {
            return Condition.Compare(col, ">=", value);
        }

        public static Condition operator <(Column col, object value)
        {
            return Condition.Compare(col, "<", value);
        }

        public static Condition operator <=(Column col, object value)
        {
            return Condition.Compare(col, "<=", value);
        }

        public static Condition operator >(object value, Column col)
        {
            return Condition.Compare(col, "<", value);
        }

        public static Condition operator >=(object value, Column col)
        {
            return Condition.Compare(col, "<=", value);
        }

        public static Condition operator <(object value, Column col)
        {
            return Condition.Compare(col, ">", value);
        }

        public static Condition operator <=(object value, Column col)
        {
            return Condition.Compare(col, ">=", value);
        }

        public Condition Contains(string value)
        {
            return Condition.Contains(this, value);
        }
    }
}
