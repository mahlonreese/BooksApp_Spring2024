namespace BooksApp_Spring2024.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<Cart> CartItems { get; set; }

        public double OrderTotal { get; set; }

    }
}
