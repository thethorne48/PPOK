﻿using System;
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
        public DateTime Expires { get; set; }

        public SystemAdminToken() { }

        public SystemAdminToken(SystemAdmin systemAdmin, string token, DateTime expires)
        {
            SystemAdmin = systemAdmin;
            Token = token;
            Expires = expires;
        }
    }
}
