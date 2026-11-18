using Library.Domain.Models;

namespace Library.Domain.Interfaces;

public interface IBookRepository
{
    List<Book> GetAllBooks();
    void SaveBook(Book book);
    void DeleteBook(int id);
}
