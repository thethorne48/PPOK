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
        public static readonly Column CodeCol = "Code";
        public static readonly Column PharmacyCodeCol = "PharmacyCode";
        public static readonly Column ContactPreferenceCol = "ContactPreference";
        public static readonly Column FirstNameCol = "FirstName";
        public static readonly Column LastNameCol = "LastName";
        public static readonly Column DOBCol = "DOB";
        public static readonly Column ZipCodeCol = "ZipCode";
        public static readonly Column PhoneCol = "Phone";
        public static readonly Column EmailCol = "Email";

        public PatientService() : base("Patient")
        {

        }
    }
}
