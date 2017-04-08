using PPOK.Domain.Types;
using System.Linq;

namespace PPOK_Twilio.Auth
{
    public class PPOKPrincipalSerializeModel : IPPOKPrincipal
    {
        public void AddToRole(string role)
        {
            if (roles.IndexOf(role) < 0)
                roles.Add(role);
        }

        public bool RemoveFromRole(string role)
        {
            return roles.Remove(role);
        }

        public PPOKPrincipalSerializeModel(Pharmacist pharmacist) : base()
        {
            Code = pharmacist.Code;
            FirstName = pharmacist.FirstName;
            LastName = pharmacist.LastName;
            Phone = pharmacist.Phone;
            Email = pharmacist.Email;
            Pharmacy = pharmacist.Jobs.First().Pharmacy;
            Type = AccountTypes.Pharmacist;
            foreach (var job in pharmacist.Jobs)
            {
                if (job.IsActive)
                {
                    if (job.IsAdmin)
                    {
                        AddToRole("Admin");
                        Type = AccountTypes.Admin;
                    }
                    AddToRole("Pharmacist");
                }
            }
        }

        public PPOKPrincipalSerializeModel(SystemAdmin admin) : base()
        {
            Code = admin.Code;
            FirstName = admin.FirstName;
            LastName = admin.LastName;
            Email = admin.Email;
            Phone = null;
            Pharmacy = new Pharmacy(0, "System Admin", "000-000-0000", "no address");
            AddToRole("System");
            Type = AccountTypes.System;
        }

        public PPOKPrincipalSerializeModel(Patient patient) : base()
        {
            Code = patient.Code;
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            Email = patient.Email;
            Phone = patient.Phone;
            Pharmacy = patient.Pharmacy;
            AddToRole("Patient");
            Type = AccountTypes.Patient;
        }

        public PPOKPrincipalSerializeModel(string email) : base()
        {
            Email = email;
        }

        public PPOKPrincipalSerializeModel() : base()
        {
        }
    }
}