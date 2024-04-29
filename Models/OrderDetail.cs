using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksApp_Spring2024.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderId { get; set; }

        [ValidateNever]
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ValidateNever]
        [ForeignKey("BookId")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
