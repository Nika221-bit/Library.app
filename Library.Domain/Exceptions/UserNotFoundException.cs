using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Exceptions
{
    internal class UserNotFoundException : Exception
    {
        public string Username { get; }
        public UserNotFoundException(string username)
            : base($"user named '{username}' not found")
        {
            Username = username;
        }
    }
}
