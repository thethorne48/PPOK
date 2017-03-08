using System;
using System.Reflection;
using System.IO;

namespace PPOK.Domain.Utility
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConfiguredAttribute : Attribute { }

    public static class Config
    {
        [Configured]
        public static string DBConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=PPOK;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        [Configured]
        public static string BotEmail = "OcPPOKEmailerTwilioBot@gmail.com";
        [Configured]
        public static string BotPassword = "PPOKEmailerBot";

        static Config()
        {
            try
            {
                typeof(Config).LoadConfig(AppDomain.CurrentDomain.BaseDirectory + "\\app.cfg");
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }






        private static readonly BindingFlags configPropFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        private static readonly string comment = "//";
        private static readonly char delimiter = '=';
        private static readonly string outputTemplate = string.Format("{{0}}{0}{{1}}", delimiter);

        public static void LoadConfig(this Type type, string file)
        {
            type.ReadConfig(file);
            type.WriteConfig(file);
        }

        public static void ReadConfig(this Type type, string file)
        {
            char[] split = new char[] { delimiter };

            if (File.Exists(file))
            {
                using (StreamReader input = new StreamReader(file))
                {
                    string line;
                    while ((line = input.ReadLine()) != null)
                    {
                        line = line.TrimStart();
                        if (line == "" || line.StartsWith(comment))
                            continue;
                        string[] parts = line.Split(split, 2);
                        string name = parts[0].Trim();
                        string value = parts[1];

                        Action<object> assign;
                        Type attr_type;

                        FieldInfo finfo = type.GetField(name, configPropFlags);
                        if (finfo != null)
                        {
                            assign = obj => finfo.SetValue(null, obj);
                            attr_type = finfo.FieldType;
                        }
                        else
                        {
                            PropertyInfo pinfo = type.GetProperty(name, configPropFlags);
                            if (pinfo != null)
                            {
                                assign = obj => pinfo.SetValue(null, obj);
                                attr_type = pinfo.PropertyType;
                            }
                            else
                            {
                                Console.WriteLine(string.Format("[Config] Unknown property {0} for class {1}", name, type.Name));
                                continue;
                            }
                        }

                        object parsed = GenericIO.Parse(value, attr_type);
                        assign(parsed);
                    }
                }
            }
        }

        public static void WriteConfig(this Type type, string file)
        {
            using (StreamWriter output = new StreamWriter(file))
            {
                foreach (var info in type.GetFields<ConfiguredAttribute>(configPropFlags))
                {
                    string name = info.Name;
                    string value = GenericIO.Serialize(info.GetValue(null));
                    output.WriteLine(string.Format(outputTemplate, name, value));
                }
                foreach (var info in type.GetProperties<ConfiguredAttribute>(configPropFlags))
                {
                    string name = info.Name;
                    string value = GenericIO.Serialize(info.GetValue(null));
                    output.WriteLine(string.Format(outputTemplate, name, value));
                }
            }
        }
    }
}
