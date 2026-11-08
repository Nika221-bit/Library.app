using System;
using System.Collections.Generic;
using System.Text;
using Library.Domain.Models;
using Library.Domain.Interfaces;


namespace Library.Aplication.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void RegisterUser(string name, string email, string password)
        {
            List<User> Users = _userRepository.GetAllUsers();


            int maxId = 0;
            if(Users.Count > 0)
            {
                maxId = Users.Max(u => u.Id);
                maxId++;
            }

            Random random = new();

            var user = new User
            {
                Id = maxId,
                Name = name,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                IsVerified = false
            };
            _userRepository.SaveUser(user);
        }

        public void LoginUser(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                Console.WriteLine("Login successful!");
            }
            else
            {
                Console.WriteLine("Invalid email or password.");
            }
        }




    }
}
