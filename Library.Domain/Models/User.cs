using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { private get; set; }

        public override string? ToString()
        {
            return $"{Id}|{Name}|{Email}|{Password}";
        }
    } 
      
}