using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Types;

namespace PPOK.Domain.Service
{
    public class PatientService : CRUDService<Patient>
    {
        public const string TABLE = "Patient";
        public static readonly Column CodeCol = new Column { table = TABLE, column = "Code" };
        public static readonly Column PharmacyCodeCol = new Column { table = TABLE, column = "PharmacyCode" };
        public static readonly Column ContactPreferenceCol = new Column { table = TABLE, column = "ContactPreference" };
        public static readonly Column FirstNameCol = new Column { table = TABLE, column = "FirstName" };
        public static readonly Column LastNameCol = new Column { table = TABLE, column = "LastName" };
        public static readonly Column DOBCol = new Column { table = TABLE, column = "DOB" };
        public static readonly Column ZipCodeCol = new Column { table = TABLE, column = "ZipCode" };
        public static readonly Column PhoneCol = new Column { table = TABLE, column = "Phone" };
        public static readonly Column EmailCol = new Column { table = TABLE, column = "Email" };

        public PatientService() : base(TABLE)
        {

        }
    }
}
