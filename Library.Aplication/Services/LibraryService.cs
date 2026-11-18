using Library.Domain.Interfaces;
using Library.Domain.Models;

namespace Library.Aplication.Services;

public sealed class LibraryService
{
    private readonly IUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;

    public LibraryService(IUserRepository userRepository, IBookRepository bookRepository)
    {
        _userRepository = userRepository;
        _bookRepository = bookRepository;
    }

    public List<User> GetUsers() => _userRepository.GetAllUsers();
    public List<Book> GetBooks() => _bookRepository.GetAllBooks();

    public void AddBook(string title, string author)
    {
        var books = GetBooks();
        _bookRepository.SaveBook(new Book
        {
            Id = books.Count == 0 ? 1 : books.Max(book => book.Id) + 1,
            Title = title,
            Author = author
        });
    }

    public void RemoveBook(int id)
    {
        var book = GetBooks().FirstOrDefault(item => item.Id == id)
            ?? throw new InvalidOperationException("Book was not found.");
        if (book.BorrowedByEmail is not null)
            throw new InvalidOperationException("A borrowed book cannot be removed.");
        _bookRepository.DeleteBook(id);
    }

    public void BorrowBook(int id, string email)
    {
        var book = GetBooks().FirstOrDefault(item => item.Id == id)
            ?? throw new InvalidOperationException("Book was not found.");
        if (book.BorrowedByEmail is not null)
            throw new InvalidOperationException("Book is already borrowed.");
        book.BorrowedByEmail = email;
        _bookRepository.SaveBook(book);
    }

    public void ReturnBook(int id, string email)
    {
        var book = GetBooks().FirstOrDefault(item => item.Id == id)
            ?? throw new InvalidOperationException("Book was not found.");
        if (!string.Equals(book.BorrowedByEmail, email, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("This book is not borrowed by you.");
        book.BorrowedByEmail = null;
        _bookRepository.SaveBook(book);
    }

    public void RemoveUser(string email)
    {
        var users = GetUsers();
        var user = users.FirstOrDefault(item =>
            string.Equals(item.Email, email, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException("User was not found.");
        users.Remove(user);
        _userRepository.SaveUsers(users.Select(item => item.ToString()).ToList());
    }

    public void UpdateUser(User user) => _userRepository.SaveUser(user);
}
