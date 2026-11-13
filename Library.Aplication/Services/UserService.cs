using Library.Domain.Models;
using Library.Domain.Interfaces;
using System.Security.Cryptography;


namespace Library.Aplication.Services
{
    public class UserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public void RegisterUser(string name, string email, string password)
        {
            var users = _userRepository.GetAllUsers();

            var nextId = users.Count == 0 ? 1 : users.Max(user => user.Id) + 1;
            var verificationCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            var user = new User
            {
                Id = nextId,
                Name = name,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                IsVerified = false,
                VerifiCationCode = verificationCode,

            };
            _userRepository.SaveUser(user);


            _emailService.SendEmail(email, "ვერიფიკაციის კოდი", $"თქვენი ვერიფიკაციის კოდია: {verificationCode}");
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

        public void VerifyUser(string email, string verificationCode)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user != null && user.VerifiCationCode == verificationCode)
            {
                user.IsVerified = true;
                _userRepository.SaveUser(user);
                Console.WriteLine("User verified successfully!");
            }
            else
            {
                Console.WriteLine("Invalid email or verification code.");
            }
        }
    }
}