using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using PPOK.Domain.Utility;

namespace PPOK.Domain.Service
{
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
        public readonly HashSet<PropertyInfo> primarySet;
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
                            throw new ArgumentException($"[CRUDService] Invalid ForeignMultiKey type for property {prop.Name}. Must be SubQuery<>.");
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

            var primaryNameSet = new HashSet<string>(primaries.Select(prop => prop.Name));
            updateNames.AddRange(
                propNames.Where(name => !primaryNameSet.Contains(name))
            );

            primarySet = new HashSet<PropertyInfo>(primaries);
        }

        public object[] GetPrimaries(object obj)
        {
            object[] prims = new object[primaries.Count];
            int i = 0;
            foreach (var info in primaries)
            {
                prims[i++] = obj == null ? null : info.GetValue(obj);
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
                if (identities.Count > 0)
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

            //foreign info
            foreach (var foreign in foreigns)
                ForeignGetInformation(columns, tables, wheres, foreign, "");

            var columnString = string.Join(",", columns);
            var tableString = string.Join("", tables);
            var whereString = string.Join(" and ", wheres);
            if (whereString == "")
                return $"Select {columnString} from [{{0}}]{tableString}";
            else

                return $"Select {columnString} from [{{0}}]{tableString} where {whereString}";
        }

        private static void ForeignGetInformation(List<string> columns, List<string> tables, List<string> wheres, PropertyInfo info, string prefix)
        {
            CRUDInfo fInfo = CRUDInfo.Get(info.PropertyType);

            string name = prefix + info.Name;

            //column names
            foreach (var local in fInfo.locals)
                columns.Add($"[{name}].[{local.Name}]");

            //where statements
            List<string> ons = new List<string>();
            string prevTable = prefix.Length == 0 ? "{0}" : prefix;
            foreach (var primary in fInfo.primaries)
                ons.Add($"[{name}].[{primary.Name}]=[{prevTable}].[{info.Name}{primary.Name}]");

            //table names
            string onString = string.Join(" and ", ons);
            tables.Add($"LEFT JOIN [{info.GetCustomAttribute<ForeignKeyAttribute>().table}] AS [{name}] ON {onString}");

            //foreign info
            foreach (var foreign in fInfo.foreigns)
                ForeignGetInformation(columns, tables, wheres, foreign, name);
        }
    }

}
