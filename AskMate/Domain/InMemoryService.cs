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

        public InMemoryUserService(){}
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

        public UserModel Register(string email, string password)
        {
            string id = Guid.NewGuid().ToString();
            UserModel user = new UserModel(id, email, password);
            return user;   
        }
        public void AddUser(UserModel user)
        {
            _users.Add(user);
        }
    }

}
