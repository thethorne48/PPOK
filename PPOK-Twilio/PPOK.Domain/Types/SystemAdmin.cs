﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class SystemAdmin
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public SystemAdmin()
        {

        }

        public SystemAdmin(string firstname, string lastname, string email, string phone, byte[] hash, byte[] salt)
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
