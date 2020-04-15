using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskMate.Models;

namespace AskMate.Domain
{
    class InMemoryUserService : IUserService
    {
        private List<User> _users = new List<User>();

        public InMemoryUserService(){}
        public List<User> GetAll()
        {
            return _users;
        }


        public User GetOne(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }

        public User Login(string email, string password)
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

        public User Register(string email, string password)
        {
            string id = Guid.NewGuid().ToString();
            User user = new User(id, email, password);
            return user;   
        }
        public void AddUser(User user)
        {
            _users.Add(user);
        }
    }

}
