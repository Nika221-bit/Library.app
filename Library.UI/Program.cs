using Library.Infrastructure.Repositories;

namespace Library.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserRepository repository = new UserRepository();
            repository.AddUser();
        }
    }
}
