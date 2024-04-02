using BooksApp_Spring2024.Data;
using BooksApp_Spring2024.Models;
using BooksApp_Spring2024.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BooksApp_Spring2024.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private BooksDbContext _dbContext;
        public CartController(BooksDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItemsList = _dbContext.Cart.Where(c => c.UserId == userId).Include(c => c.Book);

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM
            {
                CartItems = cartItemsList, 
                Order = new Order()

            }
            ;

            foreach(var cartItem in shoppingCartVM.CartItems)
            {
                cartItem.SubTotal = cartItem.Book.Price * cartItem.Quantity;

                shoppingCartVM.Order.OrderTotal += cartItem.SubTotal;

            }


            return View(shoppingCartVM);
        }

        public IActionResult IncrementByOne(int id)
        {
            Cart cart = _dbContext.Cart.Find(id);

            cart.Quantity += 1;

            _dbContext.Update(cart);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");

        }

        public IActionResult DecrementByOne(int id)
        {
            Cart cart = _dbContext.Cart.Find(id);

            

            if (cart.Quantity <= 1)
            {
                _dbContext.Remove(cart);
                _dbContext.SaveChanges();
            }
            else
            {
                cart.Quantity -= 1;
                _dbContext.Update(cart);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");

        }

        public IActionResult RemoveFromCart(int id)
        {
            Cart cart = _dbContext.Cart.Find(id);

            _dbContext.Cart.Remove(cart);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult ReviewOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItemsList = _dbContext.Cart.Where(c => c.UserId == userId).Include(c => c.Book);

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM
            {
                CartItems = cartItemsList,
                Order = new Order()

            }
            ;

            foreach (var cartItem in shoppingCartVM.CartItems)
            {
                cartItem.SubTotal = cartItem.Book.Price * cartItem.Quantity;

                shoppingCartVM.Order.OrderTotal += cartItem.SubTotal;

            }

            shoppingCartVM.Order.ApplicationUser = _dbContext.ApplicationUsers.Find(userId);

            shoppingCartVM.Order.Name = shoppingCartVM.Order.ApplicationUser.FullName;

            shoppingCartVM.Order.Name = shoppingCartVM.Order.ApplicationUser.StreetAddress;

            shoppingCartVM.Order.Name = shoppingCartVM.Order.ApplicationUser.City;

            shoppingCartVM.Order.Name = shoppingCartVM.Order.ApplicationUser.State;

            shoppingCartVM.Order.Name = shoppingCartVM.Order.ApplicationUser.PostalCode;


            return View(shoppingCartVM);
        }

    }
}
