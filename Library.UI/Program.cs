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
             
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var userRepository = new UserRepository();
            var bookRepository = new BookRepository();
            var emailService = new EmailService(configuration);
            var userService = new UserService(userRepository, emailService);
            var libraryService = new LibraryService(userRepository, bookRepository);

            var menu = new MenuView(userService, libraryService);
            menu.ShowMenu();
        }

    }
}
