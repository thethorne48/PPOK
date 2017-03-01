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
        [ForeignMultiKey("Job")]
        public IEnumerable<Job> Jobs { get; set; }
        [ForeignMultiKey("FillHistory")]
        public IEnumerable<FillHistory> Fills { get; set; }
    }
}
