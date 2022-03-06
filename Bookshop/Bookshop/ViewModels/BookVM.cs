using Bookshop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookshop.ViewModels
{
    public class BookVM
    {

        public BookVM()
        {
            Book = new Book();
            Books = new List<Book>();
        }

        public Book Book { get; set; }
        public List<Book> Books { get; set; }
        public IEnumerable<SelectListItem> AuthorSelectList { get; set; }
    }
}
