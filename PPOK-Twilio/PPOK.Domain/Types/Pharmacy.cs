﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class Pharmacy
    {
        [PrimaryKey]
        public int Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [ForeignMultiKey("Job")]
        public SubQuery<Job> AllJobs { get; set; }
        [Hide]
        public SubQuery<Job> Jobs
        {
            get
            {
                if (AllJobs == null)
                    return null;
                return AllJobs.Where(JobService.IsActiveCol == true);
            }
        }
        [ForeignMultiKey("Patient")]
        public SubQuery<Patient> Patients { get; set; }
        [ForeignKey("Pharmacist")]
        public Pharmacist LastUploader { get; set; }
        public DateTime? LastUploaded { get; set; }

        public Pharmacy() { }

        public Pharmacy(int code, string name, string phone, string address)
        {
            Code = code;
            Name = name;
            Phone = phone;
            Address = address;
        }
        public Pharmacy(string name, string phone, string address)
        {
            Name = name;
            Phone = phone;
            Address = address;
        }
    }
}
