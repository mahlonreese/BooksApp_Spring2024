using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksApp_Spring2024.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public Decimal Price { get; set; }

        public string? ImgUrl { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category? category { get; set; } //navigation property

    }
}
