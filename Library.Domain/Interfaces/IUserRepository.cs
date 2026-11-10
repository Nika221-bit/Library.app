using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Interfaces
{
    public interface IUserRepository
    {
        void SaveUser(User user);
        List<User> GetAllUsers();
        User? GetUserByEmail(string email);
        void SaveUsers(List<string> users);
        void DeleteAllUsers();


    }
}
