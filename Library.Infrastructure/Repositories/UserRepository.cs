using Library.Domain.Models;
using Library.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Library.Domain.Interfaces;
using System.IO;

namespace Library.Infrastructure.Repositories
{    
    public class UserRepository : IUserRepository
    {
        private readonly string _path = @"C:\Users\User\OneDrive\Desktop\Library.app\Library.Infrastructure\Data\UserData.txt";

        public void SaveUser(User user)
        {
            File.AppendAllLines(_path, new List<string> { user.ToString() });
        }

        public List<User> GetAllUsers()
        {
            var userlines = File.ReadAllLines(_path);
            List<User> userList = new List<User>();

            foreach (var item in userlines)
            {
                var parts = item.Split('|');
                User user = new User();
                user.Id = int.Parse(parts[0]);
                user.Name = parts[1];
                user.Email = parts[2];
                user.Password = parts[3];
                userList.Add(user);
            }
            return userList;
        }
        

        public User GetUserByEmail(string email)
        {
            var userLines = File.ReadAllLines(_path).ToList();

            foreach (var item in userLines)
            {
                var parts = item.Split("|");
                if (parts[2] == email)
                {

                    User user = new User();
                    user.Id = int.Parse(parts[0]);
                    user.Name = parts[1];
                    user.Email = parts[2];
                    user.Password = parts[3];
                    return user;

                }
            }
            return null;




        }
    }
}
