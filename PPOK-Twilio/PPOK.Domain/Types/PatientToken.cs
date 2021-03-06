﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;
using PPOK.Domain.Utility;

namespace PPOK.Domain.Types
{
    public class PatientToken
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("Patient")]
        public Patient Patient { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }

        public PatientToken() { }

        public PatientToken(Patient patient, string token)
        {
            Patient = patient;
            Token = token;
            Expires = DateTime.Now.ToUniversalTime().AddHours(Config.TokenDuration);
        }
    }
}
