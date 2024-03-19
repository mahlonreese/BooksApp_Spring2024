using System.ComponentModel.DataAnnotations.Schema;

namespace BooksApp_Spring2024.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; } //navigational property

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; } //navigational property

        public int Quantity { get; set; }

    }
}
