using Library.Aplication.Services;
using Library.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using MenuView = Library.UI.Menu.Menu;

namespace Library.UI
{
    internal static class Program
    {
        private static void Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var userRepository = new UserRepository();
            var emailService = new EmailService(configuration);
            var userService = new UserService(userRepository, emailService);

            var menu = new MenuView(userService);
            menu.ShowMenu();
        }

    }
}
