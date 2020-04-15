using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskMate.Models;

namespace AskMate.Domain
{
    class InMemoryUserService : IUserService
    {
        private List<UserModel> _users = new List<UserModel>();

        public InMemoryUserService()
        {
           _users.Add(new UserModel{Id = 1, Email = "user1", Password = "user"});
           _users.Add(new UserModel{Id = 2, Email = "user2", Password = "user" });
           _users.Add(new UserModel{Id = 3, Email = "user3", Password = "user" });
        }
        public List<UserModel> GetAll()
        {
            return _users;
        }


        public UserModel GetOne(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }

        public UserModel Login(string email, string password)
        {
            var user = GetOne(email);
            if(user == null)
            {
                return null;
            }
            if(user.Password != password)
            {
                return null;
            }
            return user;
        }
    }

}
