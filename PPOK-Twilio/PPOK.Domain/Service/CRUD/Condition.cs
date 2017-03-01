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

        public static Condition Join(Column column1, Column column2)
        {
            return new Condition { condition = $"{column1}={column2}", args = new object[0] };
        }

        public static Condition Equal(Column column, object value, bool invert = false)
        {
            if (value is Column)
                return Join(column, value as Column);
            string cmp;
            if (value is string)
                cmp = !invert ? "like" : "not like";
            else
                cmp = !invert ? "=" : "!=";
            return new Condition { condition = $"{column} {cmp} @{{0}}", args = new object[] { value } };
        }

        public static Condition Contains(Column column, string value)
        {
            return new Condition { condition = $"{column} like concat('%', @{{0}}, '%')", args = new object[] { value } };
        }
    }

    public class ConstCondition : Condition
    {
        public bool value;

        internal ConstCondition() { }
    }
}
