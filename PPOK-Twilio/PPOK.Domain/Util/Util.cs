using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Dynamic;

namespace PPOK.Domain.Utility
{
    public static class Util
    {
        public static V Getsert<K, V>(Dictionary<K, V> dict, K key, Func<K, V> factory)
        {
            V value;
            if (dict.TryGetValue(key, out value))
                return value;
            value = factory(key);
            dict[key] = value;
            return value;
        }

        public static bool HasAttribute<T>(this FieldInfo info) where T : Attribute
        {
            return info.GetCustomAttribute<T>() != null;
        }

        public static bool HasAttribute(this FieldInfo info, Type type)
        {
            return info.GetCustomAttribute(type) != null;
        }

        public static bool HasAttribute<T>(this PropertyInfo info) where T : Attribute
        {
            return info.GetCustomAttribute<T>() != null;
        }

        public static bool HasAttribute(this PropertyInfo info, Type type)
        {
            return info.GetCustomAttribute(type) != null;
        }

        public static FieldInfo[] GetFields<T>(this Type type, BindingFlags flags) where T : Attribute
        {
            return type.GetFields(flags).Where(info => info.HasAttribute<T>()).ToArray();
        }

        public static PropertyInfo[] GetProperties<T>(this Type type, BindingFlags flags) where T : Attribute
        {
            return type.GetProperties(flags).Where(info => info.HasAttribute<T>()).ToArray();
        }

        public static ExpandoObject DictToObject<T>(Dictionary<string, T> dict)
        {
            IDictionary<string, object> args = new ExpandoObject();
            foreach (var entry in dict)
                args[entry.Key] = entry.Value;
            return args as ExpandoObject;
        }

        public static string NamedFormat(string str, object args)
        {
            var parts = new List<string>();
            int start = 0;
            for (int index = 0; index < str.Length; index++)
            {
                if (str[index] == '{')
                {
                    parts.Add(str.Substring(start, index - start));
                    index++;
                    if (index == str.Length)
                        throw new ArgumentException("[NamedFormat] Unbalanced expression opener.");
                    if (str[index] == '{')
                    {
                        parts.Add("{");
                    }
                    else
                    {
                        parts.Add(GetExpression(str, ref index, args));
                    }
                    start = index + 1;
                }
                else
                if (str[index] == '}')
                {
                    parts.Add(str.Substring(start, index - start));
                    index++;
                    if (index == str.Length)
                        throw new ArgumentException("[NamedFormat] Unbalanced expression closer.");
                    if (str[index] == '}')
                    {
                        parts.Add("}");
                    }
                    else
                    {
                        throw new ArgumentException("[NamedFormat] Unbalanced expression closer.");
                    }
                    start = index + 1;
                }
            }
            parts.Add(str.Substring(start));

            return string.Join("", parts);
        }

        private static string GetExpression(string str, ref int index, object args)
        {
            int start = index;
            char c;
            for (; index < str.Length; index++)
            {
                c = str[index];
                if (c == '}')
                {
                    string exp = str.Substring(start, index - start);
                    if (exp == "")
                        throw new ArgumentException("[NamedFormat] Invalid empty expression.");
                    object value = args;
                    foreach (string prop in exp.Split('.'))
                    {
                        if (value == null)
                            throw new NullReferenceException($"[NamedFormat] Null pointer exception in \"{exp}\"");
                        FieldInfo finfo = value.GetType().GetField(prop);
                        if (finfo != null)
                        {
                            value = finfo.GetValue(value);
                        }
                        else
                        {
                            PropertyInfo pinfo = value.GetType().GetProperty(prop);
                            if (pinfo != null)
                            {
                                value = pinfo.GetValue(value);
                            }
                            else
                            {
                                throw new ArgumentException($"[NamedFormat] Unknown argument \"{exp}\"");
                            }
                        }
                    }
                    return value != null ? value.ToString() : null;
                }
                if (!InRange(c, 'a', 'z') && !InRange(c, 'A', 'Z') && !InRange(c, '0', '9') && c != '_' && c != '.')
                    throw new ArgumentException($"[NamedFormat] Invalid expression character '{c}'.");
            }
            throw new ArgumentException("[NamedFormat] Unbalanced expression opener.");
        }

        private static bool InRange(char c, char min, char max)
        {
            return c >= min && c <= max;
        }
    }
}
