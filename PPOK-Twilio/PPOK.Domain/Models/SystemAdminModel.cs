using PPOK.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOK.Domain.Models
{
    public class SystemAdminModel
    {
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public SystemAdminModel(SystemAdmin a)
        {
            Code = a.Code;
            FirstName = a.FirstName;
            LastName = a.LastName;
            Email = a.Email;
            Phone = a.Phone;
        }
    }
}
