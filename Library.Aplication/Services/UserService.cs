using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Aplication.Services
{
    public class UserService
    {

        private readonly string _usersFilePath = "users.txt";

        public void UserAdd(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username or password cannot be empty.");
            }

            if (password.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters long.");
            }

            bool userExists = CheckIfUserExistsInFile(username);

            if (userExists)
            {
                throw new ($"User with username '{username}' already exists.");
            }

            SaveUserToFile(username, password, role);
        }

        private bool CheckIfUserExistsInFile(string username)
        {
            if (!File.Exists(_usersFilePath))
            {
                return false;
            }

            try
            {
                string[] lines = File.ReadAllLines(_usersFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');

                    if (parts.Length >= 2 && parts[1].Trim().Equals(username, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            catch (IOException ex)
            {
                throw new Exception($"Error reading users file: {ex.Message}", ex);
            }

            return false;
        }

        private void SaveUserToFile(string username, string password, string role)
        {
            int newId = GenerateNewUserId();
            string userRecord = $"{newId} | {username} | {password} | {role} | 0.00\n";

            try
            {
                File.AppendAllText(_usersFilePath, userRecord);
            }
            catch (IOException ex)
            {
                throw new Exception($"Error saving user to file: {ex.Message}", ex);
            }
        }

        private int GenerateNewUserId()
        {
            if (!File.Exists(_usersFilePath))
            {
                return 1001;
            }

            try
            {
                string[] lines = File.ReadAllLines(_usersFilePath);
                if (lines.Length == 0) return 1001;

                string lastLine = lines[lines.Length - 1];
                string[] parts = lastLine.Split('|');

                if (parts.Length > 0 && int.TryParse(parts[0].Trim(), out int lastId))
                {
                    return lastId + 1;
                }
            }
            catch (Exception)
            {
                return 1001;
            }

            return 1001;
        }
    }
}
