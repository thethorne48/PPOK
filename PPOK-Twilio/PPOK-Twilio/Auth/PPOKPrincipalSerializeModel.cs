using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPOK_Twilio.Auth
{
    public class PPOKPrincipalSerializeModel
    {
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public IEnumerable<Job> Jobs { get; set; }
        public IEnumerable<FillHistory> Fills { get; set; }
        public PPOKPrincipalSerializeModel(Pharmacist pharmacist)
        {
            Code = pharmacist.Code;
            FirstName = pharmacist.FirstName;
            LastName = pharmacist.LastName;
            Phone = pharmacist.Phone;
            Email = pharmacist.Email;
            Jobs = pharmacist.Jobs;
            Fills = pharmacist.Fills;
        }
        public PPOKPrincipalSerializeModel()
        {
            Jobs = new List<Job>();
            Fills = new List<FillHistory>();
        }
    }
}