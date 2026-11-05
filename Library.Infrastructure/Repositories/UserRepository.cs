using Library.Domain.Models;
using Library.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Infrastructure.Repositories
{
    public class UserRepository
    {
        private readonly string path = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "UserData.txt");


        public void AddUser()
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("User data file was not found.", path);
            }

            var allUsers = File.ReadAllLines(path)
                               .Where(x => !string.IsNullOrWhiteSpace(x))
                               .ToList();

            int newId = allUsers.Count == 0
                ? 1
                : allUsers.Max(x => int.Parse(x.Split('|')[0])) + 1;

            User user = new User();
            user.Id = newId;
            Console.WriteLine("-----Enter user details-----");
            Console.WriteLine("Name: ");
            user.Name = Console.ReadLine();
            Console.WriteLine("Email: ");
            user.Email = Console.ReadLine();

            bool emailAlreadyExists = allUsers.Any(line =>
            {
                string[] fields = line.Split('|');
                return fields.Length > 2 &&
                       string.Equals(fields[2], user.Email, StringComparison.OrdinalIgnoreCase);
            });

            if (emailAlreadyExists)
            {
                throw new UserAlreadyExistsException(
                    $"A user with email '{user.Email}' already exists.");
            }

            Console.WriteLine("Password: ");
            user.Password = Console.ReadLine();
            File.AppendAllLines(path, [user.ToString()]);
            Console.WriteLine("User added successfully.");


        }


    }
}
