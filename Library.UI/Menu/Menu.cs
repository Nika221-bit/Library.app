using Library.Aplication.Services;
using Spectre.Console;

namespace Library.UI.Menu
{
    public class Menu
    {
        private const string RegisterOption = "Register";
        private const string LoginOption = "Login";
        private const string ExitOption = "Exit";

        private readonly UserService _service;
        private readonly LibraryService _library;

        public Menu(UserService service, LibraryService library)
        {
            _service = service;
            _library = library;
        }

        public void ShowMenu()
        {
            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText("Library").Color(Color.Cyan1));
                AnsiConsole.Write(new Rule("Account menu").RuleStyle("grey"));

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What would you like to do?")
                        .PageSize(5)
                        .AddChoices(RegisterOption, LoginOption, ExitOption));

                try
                {
                    switch (choice)
                    {
                        case RegisterOption:
                            Register();
                            break;
                        case LoginOption:
                            Login();
                            break;
                        case ExitOption:
                            return;
                    }
                }
                catch (Exception exception)
                {
                    AnsiConsole.MarkupLine($"[red]Operation failed:[/] {Markup.Escape(exception.Message)}");
                }

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Press any key to return to the menu.[/]");
                Console.ReadKey(intercept: true);
            }
        }

        private void Register()
        {
            var name = AnsiConsole.Ask<string>("Enter your name:");
            var email = AnsiConsole.Ask<string>("Enter your email:");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your password:")
                    .Secret());
            var role = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Register as:")
                    .AddChoices("User", "Admin"));
            if(role == "Admin")
            {
                var secretWord = AnsiConsole.Ask<string>("Enter the secret word for admin registration:");
                var configSecretWord = System.Text.Json.JsonDocument.Parse(File.ReadAllText("appsettings.json"))
                    .RootElement.GetProperty("SecretWordForAdmin").GetString();
                if (secretWord != configSecretWord)
                {
                    AnsiConsole.MarkupLine("[red]Invalid secret word for admin registration.[/]");
                    role = "User"; 
                }
            }
            _service.RegisterUser(name, email, password, role);
            AnsiConsole.MarkupLine("[green]Registration saved. A verification email was sent.[/]");
            if (Verify(email))
            {
                var user = _service.LoginUser(email, password);
                if (user is not null)
                {
                    AnsiConsole.MarkupLine($"[green]Welcome, {Markup.Escape(user.Name)}![/]");
                    OpenRoleMenu(user);
                }
            }
        }

        private void Login()
        {
            var email = AnsiConsole.Ask<string>("Enter your email:");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your password:")
                    .Secret());

            var user = _service.LoginUser(email, password);
            if (user is null)
            {
                AnsiConsole.MarkupLine("[red]Invalid email or password.[/]");
                return;
            }

            if (!user.IsVerified)
            {
                AnsiConsole.MarkupLine("[yellow]Your account is not verified yet.[/]");
                var code = AnsiConsole.Ask<string>("Enter the verification code sent to your email:");
                if (!_service.VerifyUser(email, code))
                {
                    AnsiConsole.MarkupLine("[red]Invalid verification code.[/]");
                    return;
                }

                AnsiConsole.MarkupLine("[green]Email verified successfully.[/]");
            }

            AnsiConsole.MarkupLine($"[green]Login successful. Welcome, {Markup.Escape(user.Name)}![/]");
            OpenRoleMenu(user);
        }

        private void OpenRoleMenu(Library.Domain.Models.User user)
        {
            if (string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                new AdminMenu(user, _library, _service).ShowMenu();
            else
                new UserMenu(user, _library).ShowMenu();
        }

        private bool Verify(string email)
        {
            var code = AnsiConsole.Ask<string>("Enter the verification code:");
            if (_service.VerifyUser(email, code))
            {
                AnsiConsole.MarkupLine("[green]User verified successfully![/]");
                return true;
            }

            AnsiConsole.MarkupLine("[red]Invalid email or verification code.[/]");
            return false;
        }

    }
}
