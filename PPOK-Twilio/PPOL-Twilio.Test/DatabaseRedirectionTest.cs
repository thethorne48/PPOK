using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;
using PPOK.Domain.Types;

namespace PPOL_Twilio.Test
{
    [TestClass]
    public class DatabaseRedirectionTest
    {
        [TestMethod]
        public void PharmacyTest1()
        {
            //create an interface that will assert queries
            TestingDatabaseInterface inter = new TestingDatabaseInterface();
            //tell the database to use our interface
            DatabaseService.RedirectDatabase(() => inter);

            int code = 1;
            string name = "TestPharmacy";
            string address = "#### Street, City, State, Zip";
            string phone = "123-456-7890";

            inter.Expect(
                "Insert Into [Pharmacy]([Code],[Name],[Phone],[Address]) VALUES (@Code,@Name,@Phone,@Address)", 
                new { Code = code, Name = name, Address = address, Phone = phone }
            );
            foreach(var entry in PharmacyService.defaultMessageTemplates)
            {
                inter.Expect(
                    "Insert Into [MessageTemplate]([PharmacyCode],[Type],[Media],[Content]) OUTPUT INSERTED.[Code] VALUES (@PharmacyCode,@Type,@Media,@Content)",
                    new { PharmacyCode = code, Type = entry.Key.Type, Media = entry.Key.Media, Content = entry.Value },
                    new dynamic[] { new Dictionary<string, object>() { { "Code", 1 } } }
                );
            }
            using (var service = new PharmacyService())
            {
                Pharmacy pharm = new Pharmacy { Code = code, Name = name, Address = address, Phone = phone };
                service.Create(pharm);
            }
        }
    }
}
