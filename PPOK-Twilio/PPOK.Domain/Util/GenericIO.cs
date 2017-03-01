using System;
using System.Collections.Generic;
using System.Reflection;

namespace PPOK.Domain.Utility
{
    public static class GenericIO
    {
        private delegate object Parser(string str);
        private static readonly Dictionary<Type, Parser> parsers = new Dictionary<Type, Parser>();
        private static readonly BindingFlags parserFlags = BindingFlags.Static | BindingFlags.Public;
        private static readonly Type[] parserArgTypes = new Type[] { typeof(string) };

        public static T Parse<T>(string str)
        {
            return (T)Parse(str, typeof(T));
        }

        public static object Parse(string str, Type type)
        {
            if (str == "")
                return null;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = type.GetGenericArguments()[0];
            var parser = Util.Getsert(parsers, type, GetParser);
            if (parser == null)
                return null;
            return parser(str);
        }

        public static string Serialize<T>(T value)
        {
            //TODO: Make this overridable
            return value.ToString();
        }

        private static Parser GetParser(Type type)
        {
            if (type == typeof(string))
                return str => str;
            if (type.IsEnum)
                return str => Enum.Parse(type, str);

            //asks the type if it has a parser function that takes a single string
            MethodInfo parser = type.GetMethod("Parse", parserFlags, null, parserArgTypes, null);
            if (parser == null)
                return null;
            return str => parser.Invoke(null, new object[] { str });
        }
    }
}
