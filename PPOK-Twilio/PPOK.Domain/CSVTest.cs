﻿using PPOK.Domain.Service;
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
            try
            {
                InitDatabaseService init = new InitDatabaseService();
                init.Reset();
                Types.Pharmacy pharm = new Types.Pharmacy(1, "test1", "test2", "test3");
                using (var service = new PharmacyService())
                {
                     service.Create(pharm);
                }
                init.LoadFromFile(@"..\..\App_Data\Scrubbed_Data.xlsx - Sheet1.csv", pharm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
    }
}
