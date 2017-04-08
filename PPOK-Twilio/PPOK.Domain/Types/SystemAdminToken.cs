using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class SystemAdminToken
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        [ForeignKey("SystemAdmin")]
        public SystemAdmin SystemAdmin { get; set; }
        public string Token { get; set; }

        public SystemAdminToken() { }

        public SystemAdminToken(SystemAdmin systemAdmin, string token)
        {
            SystemAdmin = systemAdmin;
            Token = token;
        }
    }
}
