using PPOK.Domain.Service;
using PPOK.Domain.Types;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
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
        public IEnumerable<Job> Jobs { get; set; }
        public IEnumerable<FillHistory> Fills { get; set; }

        public bool IsInRole(string role)
        {
            return Roles.IsUserInRole(role);
        }

    }

    public class PPOKPrincipal : IPPOKPrincipal
    {
        public PPOKPrincipal(string email) : base()
        {
            Identity = new GenericIdentity(email);
        }

        public static bool IsValid(string email, string password)
        {
            using (var db = new PharmacistService())
            {
                var pharmacist = db.GetWhere(PharmacistService.EmailCol == email).FirstOrDefault();

                if (pharmacist.Email == null)
                    return false;

                var salt = CreateSalt(5);
                var hash = GenerateSaltedHash(Encoding.ASCII.GetBytes(password), Encoding.ASCII.GetBytes("salt"));
                var hashS = Encoding.ASCII.GetString(hash); // "Qy67o89W0ucvlnYFQAc/GGcInhR69QEJV4L1GdebK+g=", "C.???V??/?v\u0005@\a?\u0018g\b?\u0014z?\u0001\tW??\u0019??+?"
                return CompareByteArrays(pharmacist.PasswordHash, GenerateSaltedHash(Encoding.ASCII.GetBytes(password), Encoding.ASCII.GetBytes("salt")));
            }
        }

        public static byte[] HashPassword(string password)
        {
            var salt = CreateSalt(5);
            return GenerateSaltedHash(Encoding.ASCII.GetBytes(password), Encoding.ASCII.GetBytes("salt"));
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