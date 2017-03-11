using System;
using System.Collections.Generic;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class Patient
    {
        [PrimaryKey]
        public int Code { get; set; }
        [ForeignKey("Pharmacy")]
        public Pharmacy Pharmacy { get; set; }
        public ContactPreference ContactPreference { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DOB { get; set; }
        public string ZipCode { get; set; }
        [ForeignMultiKey("Prescription")]
        public IEnumerable<Prescription> Prescriptions { get; set; }

        [Hide]
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public Patient()
        {

        }

        public Patient(int code, string fName, string lName, DateTime dob, string zipcode, string phone, string email)
        {
            Code = code;
            FirstName = fName;
            LastName = lName;
            Email = email;
            Phone = phone;
            DOB = dob;
            ZipCode = zipcode;
        }

        public override string ToString()
        {
            return $"[{FirstName} {LastName}, {Email}, {Phone}, {DOB}, {ZipCode}]";
        }
    }
}
