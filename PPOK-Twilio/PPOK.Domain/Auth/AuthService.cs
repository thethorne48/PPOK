using PPOK.Domain.Service;
using PPOK.Domain.Types;
using PPOK.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Auth
{
    public static class AuthService
    {
        public static Patient SendPatientToken(string phone, string token)
        {
            using (var service = new PatientService())
            {
                var patient = service.GetWhere(PatientService.PhoneCol == phone).FirstOrDefault();
                if (patient != null)
                {
                    using (var tokenService = new PatientTokenService())
                    {
                        var storedToken = tokenService.GetWhere(PatientTokenService.PatientCodeCol == patient.Code).FirstOrDefault();
                        if (storedToken == null)
                            tokenService.Create(new PatientToken(patient, token));
                        else
                        {
                            storedToken.Token = token;
                            storedToken.Expires = DateTime.Now.ToUniversalTime().AddHours(Config.TokenDuration);
                            tokenService.Update(storedToken);
                        }
                    }
                    TwilioService.SendSMSMessage(patient.Phone, "Please enter this token to login: " + token);
                }
                return patient;
            }
        }
        public static Pharmacist SendPharmacistToken(string email, string token)
        {
            using (var service = new PharmacistService())
            {
                var pharmacist = service.GetWhere(PharmacistService.EmailCol == email).FirstOrDefault();
                if (pharmacist != null)
                {
                    using (var tokenService = new PharmacistTokenService())
                    {
                        var storedToken = tokenService.GetWhere(PharmacistTokenService.PharmacistCodeCol == pharmacist.Code).FirstOrDefault();
                        if (storedToken == null)
                            tokenService.Create(new PharmacistToken(pharmacist, token));
                        else
                        {
                            storedToken.Token = token;
                            storedToken.Expires = DateTime.Now.ToUniversalTime().AddHours(Config.TokenDuration);
                            tokenService.Update(storedToken);
                        }
                    }
                    AddSystemAdminToken(email, token);
                    TwilioService.SendSMSMessage(pharmacist.Phone, "Please enter this token to login: " + token);
                }
                return pharmacist;
            }
        }
        private static SystemAdmin AddSystemAdminToken(string email, string token)
        {
            using (var service = new SystemAdminService())
            {
                var sysAdmin = service.GetWhere(SystemAdminService.EmailCol == email).FirstOrDefault();
                if (sysAdmin != null)
                {
                    using (var tokenService = new SystemAdminTokenService())
                    {
                        var storedToken = tokenService.GetWhere(SystemAdminTokenService.SystemAdminCodeCol == sysAdmin.Code).FirstOrDefault();
                        if (storedToken == null)
                            tokenService.Create(new SystemAdminToken(sysAdmin, token));
                        else
                        {
                            storedToken.Token = token;
                            storedToken.Expires = DateTime.Now.ToUniversalTime().AddHours(Config.TokenDuration);
                            tokenService.Update(storedToken);
                        }
                    }
                }
                return sysAdmin;
            }
        }
        public static SystemAdmin SendSystemAdminToken(string email, string token)
        {
            var sysAdmin = AddSystemAdminToken(email, token);
            TwilioService.SendSMSMessage(sysAdmin.Phone, "Please enter this token to login: " + token);
            return sysAdmin;
        }
        public static Patient VerifyPatientToken(string token)
        {
            using (var service = new PatientTokenService())
            {
                var patientToken = service.GetWhere(PatientTokenService.TokenCol == token).FirstOrDefault();
                if (patientToken != null && patientToken.Expires > DateTime.Now.ToUniversalTime())
                {
                    service.Delete(patientToken.Code);
                    return patientToken.Patient;
                }
                return null;
            }
        }
        public static Pharmacist VerifyPharmacistToken(string token)
        {
            using (var service = new PharmacistTokenService())
            {
                var pharmacistToken = service.GetWhere(PharmacistTokenService.TokenCol == token).FirstOrDefault();
                if (pharmacistToken != null && pharmacistToken.Expires > DateTime.Now.ToUniversalTime())
                {
                    return pharmacistToken.Pharmacist;
                }
                return null;
            }
        }
        public static SystemAdmin VerifySystemAdminToken(string token)
        {
            using (var service = new SystemAdminTokenService())
            {
                var adminToken = service.GetWhere(SystemAdminTokenService.TokenCol == token).FirstOrDefault();
                if (adminToken != null && adminToken.Expires > DateTime.Now.ToUniversalTime())
                {
                    return adminToken.SystemAdmin;
                }
                return null;
            }
        }
        public static bool ResetSystemAdminPassword(string token, SystemAdmin admin, byte[] newPasswordHash)
        {
            using (var service = new SystemAdminTokenService())
            {
                var adminToken = service.GetWhere(SystemAdminTokenService.TokenCol == token).FirstOrDefault();
                if (adminToken != null && admin != null && adminToken.Expires > DateTime.Now.ToUniversalTime() && adminToken.Code == admin.Code)
                {
                    service.Delete(adminToken);
                    using (var adminService = new SystemAdminService())
                    {
                        admin.PasswordHash = newPasswordHash;
                        adminService.Update(admin);
                    }
                    return true;
                }
                return false;
            }
        }
        public static bool ResetPharmacistPassword(string token, Pharmacist pharmacist, byte[] newPasswordHash)
        {
            using (var service = new PharmacistTokenService())
            {
                var pharmacistToken = service.GetWhere(PharmacistTokenService.TokenCol == token).FirstOrDefault();
                if (pharmacistToken != null && pharmacist != null && pharmacistToken.Expires > DateTime.Now.ToUniversalTime() && pharmacistToken.Pharmacist.Code == pharmacist.Code)
                {
                    service.Delete(pharmacistToken);
                    using (var pharmacistService = new PharmacistService())
                    {
                        pharmacist.PasswordHash = newPasswordHash;
                        pharmacistService.Update(pharmacist);
                    }
                    return true;
                }
                return false;
            }
        }
    }
}
