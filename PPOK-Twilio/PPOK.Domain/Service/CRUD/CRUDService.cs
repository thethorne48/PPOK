using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper;
using PPOK.Domain.Utility;

namespace PPOK.Domain.Service
{
    public class CRUDService<T> : DatabaseService where T : new()
    {
        private static readonly string CreateSQLTemplate;
        private static readonly string GetSQLTemplate;
        private static readonly string GetALLSQLTemplate;
        private static readonly string GetWhereSQLTemplate;
        private static readonly string UpdateSQLTemplate;
        private static readonly string DeleteSQLTemplate;
        private static readonly string SubquerySQLTemplate;
        private static readonly CRUDInfo info = CRUDInfo.Get(typeof(T));

        static CRUDService()
        {
            CreateSQLTemplate = info.CreateSQLTemplate;
            GetSQLTemplate = info.GetSQLTemplate;
            GetALLSQLTemplate = info.GetAllSQLTemplate;
            GetWhereSQLTemplate = info.GetWhereSQLTemplate;
            UpdateSQLTemplate = info.UpdateSQLTemplate;
            DeleteSQLTemplate = info.DeleteSQLTemplate;
            SubquerySQLTemplate = info.SubquerySQLTemplate;
        }

        private static object ToArgs(T value)
        {
            if (info.foreigns.Count == 0)
                return value;
            DynamicParameters args = new DynamicParameters();
            foreach (var prop in info.locals)
                args.Add(prop.Name, prop.GetValue(value));
            foreach (var prop in info.foreigns)
            {
                object obj = prop.GetValue(value);
                var subInfo = CRUDInfo.Get(prop.PropertyType);
                object[] prims = subInfo.GetPrimaries(obj);

                int i = 0;
                foreach (var primProp in subInfo.primaries)
                    args.Add($"{prop.Name}{primProp.Name}", prims[i++]);
            }
            return args;
        }

        private static object ToArgs(object[] primaries)
        {
            if (primaries.Length != info.primaries.Count)
                throw new ArgumentException("[CRUDService] Invalid number of arguments.");
            DynamicParameters args = new DynamicParameters();
            for (int i = 0; i < primaries.Length; i++)
                args.Add(info.primaries[i].Name, primaries[i]);
            return args;
        }

        private static T FromArgs(dynamic row)
        {
            if(row == null)
                return default(T);
            var iter = ((IDictionary<string, object>)row).Select(entry => entry.Value).GetEnumerator();
            T obj = new T();
            bool isNull;
            FromArgs(obj, iter, info, out isNull);
            if (isNull)
                obj = default(T);
            return obj;
        }

        private static void PopulateIdentities(T obj, dynamic row)
        {
            var iter = ((IDictionary<string, object>)row).Select(entry => entry.Value).GetEnumerator();
            foreach(var identity in info.identities)
            {
                iter.MoveNext();
                identity.SetValue(obj, iter.Current);
            }
        }

        private static void FromArgs(object obj, IEnumerator<object> iter, CRUDInfo info, out bool isNull)
        {
            isNull = false;
            //column names
            foreach (var local in info.locals)
            {
                iter.MoveNext();
                local.SetValue(obj, iter.Current);
                if (iter.Current == null && info.primarySet.Contains(local))
                    isNull = true;
            }

            //foreign info
            foreach (var foreign in info.foreigns)
            {
                CRUDInfo fInfo = CRUDInfo.Get(foreign.PropertyType);
                object inst = fInfo.constructor();
                bool isForeignNull;
                FromArgs(inst, iter, fInfo, out isForeignNull);
                if (isForeignNull)
                    inst = null;
                foreign.SetValue(obj, inst);
            }

            //subquery info
            foreach (var subquery in info.subqueries)
            {
                var attr = subquery.GetCustomAttribute<ForeignMultiKeyAttribute>();
                var fInfo = CRUDInfo.Get(subquery.PropertyType.GetGenericArguments()[0]);

                string table = attr.table;
                string condition = string.Format(SubquerySQLTemplate, attr.prefix ?? obj.GetType().Name);
                object[] primaries = info.GetPrimaries(obj);

                Condition cond = new Condition(condition, primaries);

                object subqueryObj = fInfo.subqueryConstructor(table, cond);
                subquery.SetValue(obj, subqueryObj);
            }
        }

        private readonly string CreateSQL;
        private readonly string GetSQL;
        private readonly string GetALLSQL;
        private readonly string GetWhereSQL;
        private readonly string UpdateSQL;
        private readonly string DeleteSQL;

        protected internal CRUDService(string table)
        {
            CreateSQL = string.Format(CreateSQLTemplate, table);
            GetSQL = string.Format(GetSQLTemplate, table);
            GetALLSQL = string.Format(GetALLSQLTemplate, table);
            GetWhereSQL = string.Format(GetWhereSQLTemplate, table);
            UpdateSQL = string.Format(UpdateSQLTemplate, table);
            DeleteSQL = string.Format(DeleteSQLTemplate, table);
        }

        public virtual void Create(T obj)
        {
            object args = ToArgs(obj);
            if(info.identities.Count == 0)
            {
                conn.Execute(CreateSQL, args);
            }else
            {
                var row = conn.Query(CreateSQL, args).FirstOrDefault();
                PopulateIdentities(obj, row);
            }
        }

        public virtual void Create(params T[] objs)
        {
            Create(objs);
        }

        public virtual void Create(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
                Create(obj);
        }

        public virtual T Get(params object[] primaries)
        {
            var args = ToArgs(primaries);
            var row = conn.Query(GetSQL, args).FirstOrDefault();
            return FromArgs(row);
        }

        public virtual List<T> GetAll()
        {
            var rows = conn.Query(GetALLSQL);
            return rows.Select(FromArgs).AsList();
        }

        public virtual List<T> GetWhere(string condition, object param)
        {
            string query = string.Format(GetWhereSQL, condition);
            var rows = conn.Query(query, param);
            return rows.Select(FromArgs).AsList();
        }

        public List<T> GetWhere(Condition condition)
        {
            if (condition is ConstCondition)
            {
                if ((condition as ConstCondition).value)
                    return GetAll();
                else
                    return new List<T>();
            }
            var info = condition.GetQueryInfo();
            return GetWhere(info.Item1, info.Item2);
        }

        public virtual void Update(T obj)
        {
            object args = ToArgs(obj);
            conn.Execute(UpdateSQL, args);
        }

        public virtual void Update(params T[] objs)
        {
            Update(objs);
        }

        public virtual void Update(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
                Update(obj);
        }

        public virtual void Delete(params object[] primaries)
        {
            var args = ToArgs(primaries);
            conn.Execute(DeleteSQL, args);
        }
    }
}
