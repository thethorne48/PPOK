using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class Pharmacist
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        [ForeignMultiKey("Job")]
        public SubQuery<Job> AllJobs { get; set; }
        [Hide]
        public SubQuery<Job> Jobs
        {
            get
            {
                return AllJobs ?? AllJobs.Where(JobService.IsActiveCol == true);
            }
        }
        [ForeignMultiKey("FillHistory")]
        public SubQuery<FillHistory> Fills { get; set; }

        public Pharmacist()
        {

        }

        public Pharmacist(string firstname, string lastname, string email, string phone, byte[] hash, byte[] salt)
        {
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            Phone = phone;
            PasswordHash = hash;
            PasswordSalt = salt;
        }
    }
}
