using Library.Domain.Exceptions;
using Library.Aplication.Services;
using Library.Infrastructure.Repositories;

namespace Library.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserRepository repository = new UserRepository();

            try
            {
                repository.AddUser();
            }
            catch (UserAlreadyExistsException exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
            }
        }
    }
}
