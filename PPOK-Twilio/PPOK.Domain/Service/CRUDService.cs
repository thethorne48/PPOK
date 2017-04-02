using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper;
using PPOK.Domain.Utility;

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

        public ForeignMultiKeyAttribute(string table, string prefix=null)
        {
            this.table = table;
            this.prefix = prefix;
        }
    }

    internal class CRUDInfo
    {
        private static readonly Dictionary<Type, CRUDInfo> infos = new Dictionary<Type, CRUDInfo>();
        private static readonly Type[] noTypes = new Type[0];
        private static readonly object[] noArgs = new object[0];
        private static readonly Type[] subqueryArgs = new Type[] { typeof(string), typeof(Condition) };

        public static CRUDInfo Get(Type type)
        {
            CRUDInfo info;
            if (infos.TryGetValue(type, out info))
                return info;
            return infos[type] = new CRUDInfo(type);
        }

        public readonly Func<object> constructor;
        public readonly Func<string, Condition, object> subqueryConstructor;
        public readonly List<PropertyInfo> primaries = new List<PropertyInfo>();
        public readonly List<PropertyInfo> identities = new List<PropertyInfo>();
        public readonly List<PropertyInfo> foreigns = new List<PropertyInfo>();
        public readonly List<PropertyInfo> subqueries = new List<PropertyInfo>();
        public readonly List<PropertyInfo> locals = new List<PropertyInfo>();
        public readonly List<PropertyInfo> props = new List<PropertyInfo>();
        public readonly List<string> propNames = new List<string>();
        public readonly List<string> createNames = new List<string>();
        public readonly List<string> updateNames = new List<string>();

        private CRUDInfo(Type type)
        {
            ConstructorInfo info = type.GetConstructor(noTypes);
            constructor = () => info.Invoke(noArgs);

            Type subType = typeof(SubQuery<>).MakeGenericType(type);
            ConstructorInfo subInfo = subType.GetConstructor(subqueryArgs);
            subqueryConstructor = (table, condition) => subInfo.Invoke(new object[] { table, condition });

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.HasAttribute<HideAttribute>())
                {
                    if (prop.HasAttribute<ForeignMultiKeyAttribute>())
                    {
                        if (prop.HasAttribute<PrimaryKeyAttribute>())
                            throw new ArgumentException("[CRUDService] Primary foreign multi keys are not supported!");
                        if (!prop.PropertyType.IsGenericType || prop.PropertyType.GetGenericTypeDefinition() != typeof(SubQuery<>))
                            throw new ArgumentException($"[CRUDService] Invalid ForeignMultiKey type for property {prop.Name}. Must be IEnumerable<>.");
                        subqueries.Add(prop);
                    }
                    else
                    {
                        props.Add(prop);
                        if (prop.HasAttribute<PrimaryKeyAttribute>())
                            primaries.Add(prop);
                        if (prop.HasAttribute<IdentityAttribute>())
                            identities.Add(prop);
                        if (prop.HasAttribute<ForeignKeyAttribute>())
                        {
                            if (prop.HasAttribute<PrimaryKeyAttribute>())
                                throw new ArgumentException("[CRUDService] Primary foreign keys are not supported!");
                            foreigns.Add(prop);
                            propNames.AddRange(
                                CRUDInfo.Get(prop.PropertyType).primaries.Select(
                                    primary =>
                                        $"{prop.Name}{primary.Name}"
                                )
                            );
                        }
                        else
                        {
                            propNames.Add(prop.Name);
                        }
                    }
                }
            }
            locals.AddRange(props);
            var foreignSet = new HashSet<PropertyInfo>(foreigns);
            locals.RemoveAll(value => foreignSet.Contains(value));

            var identitySet = new HashSet<string>(identities.Select(prop => prop.Name));
            createNames.AddRange(
                propNames.Where(name => !identitySet.Contains(name))
            );

            var primarySet = new HashSet<string>(primaries.Select(prop => prop.Name));
            updateNames.AddRange(
                propNames.Where(name => !primarySet.Contains(name))
            );
        }

        public object[] GetPrimaries(object obj)
        {
            object[] prims = new object[primaries.Count];
            int i = 0;
            foreach (var info in primaries)
            {
                prims[i++] = info.GetValue(obj);
            }
            return prims;
        }

        public string CreateSQLTemplate
        {
            get
            {
                var columns = createNames.Select(prop => $"[{prop}]");
                var paras = createNames.Select(prop => $"@{prop}");

                var columnString = string.Join(",", columns);
                var paraString = string.Join(",", paras);

                string identityString = "";
                if(identities.Count > 0)
                {
                    var idens = identities.Select(prop => $"INSERTED.[{prop.Name}]");
                    identityString = string.Join(",", idens);
                    identityString = $" OUTPUT {identityString}";
                }

                return $"Insert Into [{{0}}]({columnString}){identityString} VALUES ({paraString})";
            }
        }

        public string GetSQLTemplate
        {
            get
            {
                var columns = new List<string>();
                var tables = new List<string>();
                var wheres = new List<string>();

                //where statements
                foreach (var primary in primaries)
                    wheres.Add($"[{{0}}].[{primary.Name}]=@{primary.Name}");

                return QueryFromGetInformation(columns, tables, wheres);
            }
        }

        public string GetAllSQLTemplate
        {
            get
            {
                var columns = new List<string>();
                var tables = new List<string>();
                var wheres = new List<string>();
                return QueryFromGetInformation(columns, tables, wheres);
            }
        }

        public string GetWhereSQLTemplate
        {
            get
            {
                var columns = new List<string>();
                var tables = new List<string>();
                var wheres = new List<string>();

                //where statements
                wheres.Add("{{0}}");

                return QueryFromGetInformation(columns, tables, wheres);
            }
        }

        public string UpdateSQLTemplate
        {
            get
            {
                var updates = updateNames.Select(prop => $"[{prop}]=@{prop}");
                var wheres = new List<string>();

                //where statements
                foreach (var primary in primaries)
                    wheres.Add($"[{primary.Name}]=@{primary.Name}");

                var updateString = string.Join(",", updates);
                var whereString = string.Join(" and ", wheres);

                return $"Update [{{0}}] set {updateString} where {whereString}";
            }
        }

        public string DeleteSQLTemplate
        {
            get
            {
                var wheres = new List<string>();

                //where statements
                foreach (var primary in primaries)
                    wheres.Add($"[{primary.Name}]=@{primary.Name}");

                var whereString = string.Join(" and ", wheres);

                return $"Delete from [{{0}}] where {whereString}";
            }
        }

        public string SubquerySQLTemplate
        {
            get
            {
                var wheres = new List<string>();

                //where statements
                int i = 0;
                foreach (var primary in primaries)
                    wheres.Add($"[{{0}}{primary.Name}]=@{{{{{i++}}}}}");

                var whereString = string.Join(" and ", wheres);

                return whereString;
            }
        }

        private string QueryFromGetInformation(List<string> columns, List<string> tables, List<string> wheres)
        {
            //column names
            foreach (var local in locals)
                columns.Add($"[{{0}}].[{local.Name}]");

            //table names
            tables.Add("[{0}]");

            //foreign info
            foreach (var foreign in foreigns)
                ForeignGetInformation(columns, tables, wheres, foreign, "");

            var columnString = string.Join(",", columns);
            var tableString = string.Join(",", tables);
            var whereString = string.Join(" and ", wheres);
            if (whereString == "")
                return $"Select {columnString} from {tableString}";
            else

                return $"Select {columnString} from {tableString} where {whereString}";
        }

        private static void ForeignGetInformation(List<string> columns, List<string> tables, List<string> wheres, PropertyInfo info, string prefix)
        {
            CRUDInfo fInfo = CRUDInfo.Get(info.PropertyType);

            string name = prefix + info.Name;

            //column names
            foreach (var local in fInfo.locals)
                columns.Add($"[{name}].[{local.Name}]");

            //table names
            tables.Add($"[{info.GetCustomAttribute<ForeignKeyAttribute>().table}] AS [{name}]");

            //where statements
            string prevTable = prefix.Length == 0 ? "{0}" : prefix;
            foreach (var primary in fInfo.primaries)
                wheres.Add($"[{name}].[{primary.Name}]=[{prevTable}].[{info.Name}{primary.Name}]");

            //foreign info
            foreach (var foreign in fInfo.foreigns)
                ForeignGetInformation(columns, tables, wheres, foreign, name);
        }
    }

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

            FromArgs(obj, iter, info);

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

        private static void FromArgs(object obj, IEnumerator<object> iter, CRUDInfo info)
        {
            //column names
            foreach (var local in info.locals)
            {
                iter.MoveNext();
                local.SetValue(obj, iter.Current);
            }

            //foreign info
            foreach (var foreign in info.foreigns)
            {
                CRUDInfo fInfo = CRUDInfo.Get(foreign.PropertyType);
                object inst = fInfo.constructor();
                FromArgs(inst, iter, fInfo);
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
            foreach (var obj in objs)
                Create(obj);
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
            foreach (var obj in objs)
                Update(obj);
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
