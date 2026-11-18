using Library.Domain.Interfaces;
using Library.Domain.Models;

namespace Library.Infrastructure.Repositories;

public sealed class BookRepository : IBookRepository
{
    private readonly string _path = Path.Combine(
        @"C:\Users\User\OneDrive\Desktop\Library.app",
        "Library.Infrastructure", "Data", "Books.txt");

    public List<Book> GetAllBooks()
    {
        if (!File.Exists(_path)) return new List<Book>();

        return File.ReadAllLines(_path)
            .Select(line => line.Split('|'))
            .Where(parts => parts.Length >= 4 && int.TryParse(parts[0], out _))
            .Select(parts => new Book
            {
                Id = int.Parse(parts[0]),
                Title = parts[1],
                Author = parts[2],
                BorrowedByEmail = string.IsNullOrWhiteSpace(parts[3]) ? null : parts[3]
            })
            .ToList();
    }

    public void SaveBook(Book book)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        var books = GetAllBooks();
        var index = books.FindIndex(existing => existing.Id == book.Id);
        if (index >= 0) books[index] = book;
        else books.Add(book);
        File.WriteAllLines(_path, books.Select(ToLine));
    }

    public void DeleteBook(int id)
    {
        var books = GetAllBooks();
        File.WriteAllLines(_path, books.Where(book => book.Id != id).Select(ToLine));
    }

    private static string ToLine(Book book) =>
        $"{book.Id}|{book.Title}|{book.Author}|{book.BorrowedByEmail ?? string.Empty}";
}
