using Library.Aplication.Services;
using System;

namespace Library.UI.Menu
{
    public class Menu
    {
        private readonly UserService _service;

        public Menu(UserService service)
        {
            _service = service;
        }

        public void ShowMenu()
        {
          
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    RegisterUser();
                    break;
                case "2":
                    LoginUser();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;



            }
        }

        public void RegisterUser()
        {
            Console.Write("Enter your name: ");
            var name = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter your email: ");
            var email = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter your password: ");
            var password = Console.ReadLine() ?? string.Empty;
            _service.RegisterUser(name, email, password);
            VerifyUser();
        }

        public void LoginUser()
        {
            Console.Write("Enter your email: ");
            var email = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter your password: ");
            var password = Console.ReadLine() ?? string.Empty;
            _service.LoginUser(email, password);
        }

        public void VerifyUser()
        {
            Console.Write("Enter your email: ");
            var email = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter the verification code: ");
            var code = Console.ReadLine() ?? string.Empty;
            _service.VerifyUser(email, code);
        }

    }
}
