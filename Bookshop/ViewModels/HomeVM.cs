using Bookshop.Models;

namespace Bookshop.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Author> Authors { get; set; }
    }
}
