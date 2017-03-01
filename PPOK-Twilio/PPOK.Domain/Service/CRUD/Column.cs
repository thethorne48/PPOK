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
            return Condition.Equal(col, value);
        }

        public static Condition operator !=(Column col, object value)
        {
            return Condition.Equal(col, value, invert: true);
        }

        public Condition Contains(string value)
        {
            return Condition.Contains(this, value);
        }
    }
}
