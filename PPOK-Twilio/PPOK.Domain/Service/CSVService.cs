using PPOK.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Reflection;
using PPOK.Domain.Utility;

namespace PPOK.Domain.Service
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CSVSurrogateAttribute : Attribute
    {
        public string target;

        public CSVSurrogateAttribute(string target)
        {
            this.target = target;
        }
    }

    public class CSVService
    {
        public static List<string> ReadFile(string file)
        {
            using (StreamReader input = new StreamReader(file))
            {
                return Lines(input);
            }
        }

        public static List<string> ReadResource(string resource)
        {
            Assembly domain = Assembly.GetExecutingAssembly();
            using (Stream stream = domain.GetManifestResourceStream(resource))
            {
                using (StreamReader input = new StreamReader(stream))
                {
                    return Lines(input);
                }
            }
        }

        public static List<string> Lines(StreamReader input)
        {
            List<string> list = new List<string>();
            string line;
            while ((line = input.ReadLine()) != null)
                list.Add(line);
            return list;
        }
    }
}
