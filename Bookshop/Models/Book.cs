using System.ComponentModel.DataAnnotations;

namespace Bookshop.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public int Price { get; set; }
        public int NumOfPages { get; set; }

        [Required,StringLength(100)]
        public string Title { get; set; } = String.Empty;

        [StringLength(200)]
        public string Description { get; set; } = String.Empty;

        public string? Image { get; set; }

        public int AuthorId { get; set; }
        public Author? Author { get; set; }


    }
}
