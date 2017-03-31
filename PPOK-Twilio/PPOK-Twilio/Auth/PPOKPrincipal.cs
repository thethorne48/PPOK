using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace PPOK_Twilio.Auth
{
    public class IPPOKPrincipal : IPrincipal
    {
        public IIdentity Identity { get; set; }
        public int Code { get; set; }
        public string FirstName  { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public List<string> roles { get; set; }

        public List<string> getRoles()
        {
            return roles;
        }

        public bool IsInRole(string role)
        {
            var checkRoles = role.Split(' ');
            foreach (var Role in checkRoles)
            {
                if (roles.IndexOf(Role) > -1)
                    return true;
            }
            return false;
        }

        public IPPOKPrincipal() : base()
        {
            roles = new List<string>();
        }

    }

    public class PPOKPrincipal : IPPOKPrincipal
    {
        public PPOKPrincipal(string email) : base()
        {
            Identity = new GenericIdentity(email);
        }

        public void addRole(string role)
        {
            if(roles.IndexOf(role) < 0)
                roles.Add(role);
        }

        public void addRole(List<string> newRoles)
        {
            foreach(string role in newRoles)
            {
                addRole(role);
            }
        }

        public static bool IsValid(string email, string password)
        {
            using (var db = new PharmacistService())
            using (var adminDB = new SystemAdminService())
            {
                var pharmacist = db.GetWhere(PharmacistService.EmailCol == email).FirstOrDefault();
                var admin = adminDB.GetWhere(SystemAdminService.EmailCol == email).FirstOrDefault();
                if (pharmacist == null && admin == null)
                    return false;

                if (admin != null)
                {
                    return CompareByteArrays(admin.PasswordHash, GenerateSaltedHash(Encoding.ASCII.GetBytes(password), admin.PasswordSalt));
                }
                if (pharmacist != null)
                {
                    return CompareByteArrays(pharmacist.PasswordHash, GenerateSaltedHash(Encoding.ASCII.GetBytes(password), pharmacist.PasswordSalt));
                }
                return false;
            }
        }

        public static byte[] HashUserText(Pharmacist pharmacist, string text)
        {
            using (var service = new PharmacistService())
            {
                var salt = service.Get(pharmacist.Code).PasswordSalt;
                return GenerateSaltedHash(Encoding.ASCII.GetBytes(text), salt);
            }
        }

        public static byte[] HashUserText(SystemAdmin admin, string text)
        {
            using (var service = new SystemAdminService())
            {
                var salt = service.Get(admin.Code).PasswordSalt;
                return GenerateSaltedHash(Encoding.ASCII.GetBytes(text), salt);
            }
        }

        public static byte[] HashPassword(Pharmacist pharmacist, string password)
        {
            using (var service = new PharmacistService())
            {
                var salt = CreateSalt(32);
                pharmacist.PasswordSalt = salt;
                pharmacist.PasswordHash = GenerateSaltedHash(Encoding.ASCII.GetBytes(password), pharmacist.PasswordSalt);
                service.Update(pharmacist);
                return pharmacist.PasswordHash;
            }
        }

        public static byte[] HashPassword(SystemAdmin admin, string password)
        {
            using (var service = new SystemAdminService())
            {
                var salt = CreateSalt(32);
                admin.PasswordSalt = salt;
                admin.PasswordHash = GenerateSaltedHash(Encoding.ASCII.GetBytes(password), admin.PasswordSalt);
                return admin.PasswordHash;
            }
        }

        public static string generateRandomCode(int size)
        {
            return Convert.ToBase64String(CreateSalt(size));
        }

        private static byte[] CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a byte array representation of the random number.
            return buff;
        }

        public static bool passwordComplexity(string password)
        {
            if (password.Length < 6)
                return false;
            var hasUpperCase = new Regex(@"/[A-Z]/").IsMatch(password);
            var hasLowerCase = new Regex(@"/[a-z]/").IsMatch(password);
            var hasNumbers = new Regex(@"/\d/").IsMatch(password);
            var hasNonalphas = new Regex(@"/\W/").IsMatch(password);
            if (hasUpperCase && hasLowerCase && hasNumbers && hasNonalphas)
                return true;
            return false;
        }


        private static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }
    }
}