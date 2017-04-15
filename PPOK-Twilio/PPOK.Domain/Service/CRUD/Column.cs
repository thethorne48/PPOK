namespace PPOK.Domain.Service
{
    public class Column
    {
        internal string name;

        public Column FromTable(string table)
        {
            return new Column { name = $"{table}.{name}" };
        }

        public override string ToString()
        {
            return name;
        }

        public static implicit operator Column(string name)
        {
            return new Column { name = name };
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
