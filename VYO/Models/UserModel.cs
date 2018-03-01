using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VYODomain.Entities;

namespace VYO.Models
{
    public class UserModel
    {
        public List<User> UserAdmin { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string Request { get; set; }
    }
}