using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMate.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserModel(string id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }
    }
}
