using System.ComponentModel.DataAnnotations;

namespace Bookshop.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(3000)]
        public string? Description { get; set; }


        public string? Image { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Author Name")]
        public string Name { get; set; } = String.Empty;
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
