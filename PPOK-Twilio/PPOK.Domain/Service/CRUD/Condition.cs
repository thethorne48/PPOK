using System;
using Dapper;

namespace PPOK.Domain.Service
{
    public class Condition
    {
        private string condition;
        private object[] args;
        private string compoundType = null;

        private Tuple<string, DynamicParameters> queryInfo = null;

        protected Condition() { }

        protected internal Condition(string condition, object[] args)
        {
            this.condition = condition;
            this.args = args;
        }

        public virtual Tuple<string, DynamicParameters> GetQueryInfo()
        {
            if (queryInfo == null)
            {
                string cond = condition;
                DynamicParameters paras = new DynamicParameters();
                string[] names = new string[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    names[i] = "a" + i;
                    paras.Add(names[i], args[i]);
                }
                cond = string.Format(cond, names);
                if (compoundType != null)
                    cond = $"({cond})";
                queryInfo = new Tuple<string, DynamicParameters>(cond, paras);
            }
            return queryInfo;
        }

        public static implicit operator Condition(bool b)
        {
            return new ConstCondition { value = b };
        }

        public static Condition operator &(Condition c1, Condition c2)
        {
            return Combine(c1, c2, "and");
        }

        public static Condition operator |(Condition c1, Condition c2)
        {
            return Combine(c1, c2, "or");
        }

        private static Condition Combine(Condition c1, Condition c2, string op)
        {
            //check for constant value
            ConstCondition cc;
            Condition co;
            cc = c1 as ConstCondition;
            co = c2;
            if (cc == null)
            {
                cc = c2 as ConstCondition;
                co = c1;
            }
            if (cc != null)
            {
                if (cc.value == (op == "and"))
                    return co;
                else
                    return cc;
            }

            int c1_len = c1.args.Length;
            int c2_len = c2.args.Length;
            string c1_condition = c1.condition;
            string c2_condition = c2.condition;
            //surround different compound conditions in parenthesis
            if (c1.compoundType != null && c1.compoundType != op)
                c1_condition = $"({c1_condition})";
            if (c2.compoundType != null && c2.compoundType != op)
                c2_condition = $"({c2_condition})";
            //increase the arg counts of all the args in c2
            if (c1_len > 0)
            {
                for (int i = c2.args.Length - 1; i >= 0; i--)
                {
                    c2_condition = c2_condition.Replace($"{{{i}}}", $"{{{i + c1_len}}}");
                }
            }
            //combine args
            object[] args;
            if (c1_len == 0)
            {
                args = c2.args;
            }
            else
            if (c2_len == 0)
            {
                args = c1.args;
            }
            else
            {
                args = new object[c1_len + c2_len];
                for (int i = 0; i < c1_len; i++)
                    args[i] = c1.args[i];
                for (int i = 0; i < c2_len; i++)
                    args[i + c1_len] = c2.args[i];
            }
            return new Condition { condition = $"{c1_condition} {op} {c2_condition}", args = args, compoundType = op };
        }

        public static Condition Compare(Column column, string op, object value)
        {
            if (value is Column)
                return new Condition { condition = $"{column} {op} {value}", args = new object[0] };
            if (value is DateTime)
                return DateCompare(column, (DateTime)value, op);
            if (value is string)
            {
                switch (op)
                {
                    case "=":
                        op = "like";
                        break;
                    case "!=":
                        op = "not like";
                        break;
                    default:
                        throw new Exception($"Unsupported operator '{op}' for type string.");
                }
            }
            return new Condition { condition = $"{column} {op} @{{0}}", args = new object[] { value } };
        }

        private static Condition DateCompare(Column column, DateTime value, string op)
        {
            return new Condition { condition = $"DATEDIFF(day, @{{0}}, {column}) {op} 0", args = new object[] { value } };
        }

        public static Condition Contains(Column column, string value)
        {
            return new Condition { condition = $"{column} like concat('%', @{{0}}, '%')", args = new object[] { value } };
        }
    }

    
}
