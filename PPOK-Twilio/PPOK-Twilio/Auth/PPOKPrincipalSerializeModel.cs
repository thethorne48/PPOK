using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPOK_Twilio.Auth
{
    public class PPOKPrincipalSerializeModel : IPPOKPrincipal
    {
        //public int Code { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Phone { get; set; }
        //public string Email { get; set; }
        //public IEnumerable<Job> Jobs { get; set; }
        //public IEnumerable<FillHistory> Fills { get; set; }
        //public List<string> roles = new List<string>();// { get; set; }

        public void AddToRole(string role)
        {
            if (roles.IndexOf(role) < 0)
                roles.Add(role);
        }

        public bool RemoveFromRole(string role)
        {
            return roles.Remove(role);
        }

        public PPOKPrincipalSerializeModel(Pharmacist pharmacist)
        {
            Code = pharmacist.Code;
            FirstName = pharmacist.FirstName;
            LastName = pharmacist.LastName;
            Phone = pharmacist.Phone;
            Email = pharmacist.Email;
            Jobs = pharmacist.Jobs;
            Fills = pharmacist.Fills;
            //Roles = new List<string>();
        }
        public PPOKPrincipalSerializeModel(SystemAdmin admin)
        {
            Code = admin.Code;
            FirstName = admin.FirstName;
            LastName = admin.LastName;
            Email = admin.Email;
            Jobs = null;
            Fills = null;
            Phone = null;
            //Roles = new List<sktring>();
        }
        public PPOKPrincipalSerializeModel()
        {
            Jobs = new List<Job>();
            Fills = new List<FillHistory>();
            //Roles = new List<string>();
        }
    }
}