using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class PharmacistToken
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("Pharmacist")]
        public Pharmacist Pharmacist { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }

        public PharmacistToken() { }

        public PharmacistToken(Pharmacist pharmacist, string token)
        {
            Pharmacist = pharmacist;
            Token = token;
            Expires = DateTime.Now.AddHours(3);
        }
    }
}
