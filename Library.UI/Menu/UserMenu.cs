using Library.Aplication.Services;
using Library.Domain.Models;
using Spectre.Console;

namespace Library.UI.Menu
{
    internal class UserMenu
    {
        private readonly User _user;
        private readonly LibraryService _library;

        public UserMenu(User user, LibraryService library)
        {
            _user = user;
            _library = library;
        }

        public void ShowMenu()
        {
            while (true)
            {
                var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title($"User menu - {Markup.Escape(_user.Name)}")
                    .AddChoices("List books", "Borrow book", "Return book", "Change profile", "Back"));

                switch (choice)
                {
                    case "List books": ListBooks(); break;
                    case "Borrow book": BorrowBook(); break;
                    case "Return book": ReturnBook(); break;
                    case "Change profile": ChangeProfile(); break;
                    case "Back": return;
                }
            }
        }

        private void ListBooks()
        {
            var books = _library.GetBooks();
        var table = new Table().Border(TableBorder.Rounded)
            .AddColumns("Id", "Title", "Author", "Status");
        foreach (var book in books)
            table.AddRow(book.Id.ToString(), book.Title, book.Author,
                book.BorrowedByEmail is null ? "Available" : "Borrowed");
        AnsiConsole.Write(table);
        }

        private void BorrowBook()
        {
            var id = AnsiConsole.Ask<int>("Book id to borrow:");
            _library.BorrowBook(id, _user.Email);
            AnsiConsole.MarkupLine("[green]Book borrowed.[/]");
        }

        private void ReturnBook()
        {
            var id = AnsiConsole.Ask<int>("Book id to return:");
            _library.ReturnBook(id, _user.Email);
            AnsiConsole.MarkupLine("[green]Book returned.[/]");
        }

        private void ChangeProfile()
        {
        _user.Name = AnsiConsole.Prompt(new TextPrompt<string>("Name:").DefaultValue(_user.Name));
        _user.Email = AnsiConsole.Prompt(new TextPrompt<string>("Email:").DefaultValue(_user.Email));
            _library.UpdateUser(_user);
            AnsiConsole.MarkupLine("[green]Profile updated.[/]");
        }
    }
}
