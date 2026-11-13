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

        public Menu(UserService service)
        {
            _service = service;
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

            _service.RegisterUser(name, email, password);
            AnsiConsole.MarkupLine("[green]Registration saved. A verification email was sent.[/]");
            Verify();
        }

        private void Login()
        {
            var email = AnsiConsole.Ask<string>("Enter your email:");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your password:")
                    .Secret());

            _service.LoginUser(email, password);
        }

        private void Verify()
        {
            var email = AnsiConsole.Ask<string>("Enter your email:");
            var code = AnsiConsole.Ask<string>("Enter the verification code:");
            _service.VerifyUser(email, code);
        }

    }
}
