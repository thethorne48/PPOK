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
        public static readonly Column CodeCol = $"[{TABLE}].[Code]";
        public static readonly Column PharmacyCodeCol = $"[{TABLE}].[PharmacyCode]";
        public static readonly Column ContactPreferenceCol = $"[{TABLE}].[ContactPreference]";
        public static readonly Column FirstNameCol = $"[{TABLE}].[FirstName]";
        public static readonly Column LastNameCol = $"[{TABLE}].[LastName]";
        public static readonly Column DOBCol = $"[{TABLE}].[DOB]";
        public static readonly Column ZipCodeCol = $"[{TABLE}].[ZipCode]";
        public static readonly Column PhoneCol = "Patient.Phone";
        public static readonly Column EmailCol = $"[{TABLE}].[Email]";

        public PatientService() : base(TABLE)
        {

        }
    }
}
