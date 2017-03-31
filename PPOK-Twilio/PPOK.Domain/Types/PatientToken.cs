using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class PatientToken
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("Patient")]
        public Patient Patient { get; set; }
        public string Token { get; set; }

        public PatientToken() { }

        public PatientToken(Patient patient, string token)
        {
            Patient = patient;
            Token = token;
        }
    }
}
