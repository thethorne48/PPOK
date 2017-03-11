using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain
{
    class CSVTest
    {
        public static void Main()
        {
            Service.InitDatabaseService init = new Service.InitDatabaseService();
            init.Reset();
            init.LoadFromFile(@"..\..\App_Data\Scrubbed_Data.xlsx - Sheet1.csv");
        }
    }
}
