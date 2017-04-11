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
        public SubQuery<Prescription> Prescriptions { get; set; }
        [ForeignMultiKey("Event")]
        public SubQuery<Event> Events { get; set; }
        [ForeignMultiKey("PatientToken")]
        public SubQuery<PatientToken> Tokens { get; set; }

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

        public Patient(int code, string fName, string lName, DateTime dob, string zipcode, string phone, string email, Pharmacy pharm)
        {
            Code = code;
            FirstName = fName;
            LastName = lName;
            Email = email;
            Phone = phone;
            DOB = dob;
            ZipCode = zipcode;
            Pharmacy = pharm;
            ContactPreference = ContactPreference.EMAIL;
        }

        public override string ToString()
        {
            return $"[{FirstName} {LastName}, {Email}, {Phone}, {DOB}, {ZipCode}]";
        }
    }
}
