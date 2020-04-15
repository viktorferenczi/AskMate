using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskMate.Models;

namespace AskMate.Domain
{
    public interface IUserService
    {
        public List<User> GetAll();
        public User GetOne(string email);
        public User Login(string email, string password);
        public User Register(string email, string password);
        public void AddUser(User user);
    }
}
