using BooksApp_Spring2024.Data;
using BooksApp_Spring2024.Models;
using BooksApp_Spring2024.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
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

            shoppingCartVM.Order.Name = shoppingCartVM.Order.Phone;

            return View(shoppingCartVM);
        }

        [HttpPost]
        [ActionName("ReviewOrder")]
        public IActionResult ReviewOrderPOST(ShoppingCartVM shoppingCartVM)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItemsList = _dbContext.Cart.Where(c => c.UserId == userId).Include(c => c.Book);

            shoppingCartVM.CartItems = cartItemsList;

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

            shoppingCartVM.Order.Name = shoppingCartVM.Order.Phone;


            shoppingCartVM.Order.OrderDate = DateOnly.FromDateTime(DateTime.Now);

            shoppingCartVM.Order.OrderStatus = "Pending";

            shoppingCartVM.Order.PaymentStatus = "Pending";

            _dbContext.Orders.Add(shoppingCartVM.Order); //adds a new order to the database
            _dbContext.SaveChanges();

            //adds each individual cart item as an order detail into the orderDetails table
            foreach(var cartItem in shoppingCartVM.CartItems)
            {
                OrderDetail orderDetail = new()
                {
                    OrderId = shoppingCartVM.Order.OrderId,
                    BookId = cartItem.BookId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Book.Price,

                 

                };

                _dbContext.OrderDetails.Add(orderDetail);

            }

            _dbContext.SaveChanges();

            //StripeConfiguration.ApiKey = "sk_test_51P6upvB0jN1vlKlXa1dfOrho5JATbg1n6y6pZVc4BDArdXhVsxFkXrm4fDwHP7p7uQqyIXkLQEE4bKNvVvXMkk7Y004AY6ipnP";

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = "https://localhost:7206/" + $"customer/cart/OrderConfirmation?id={shoppingCartVM.Order.OrderId}",

                CancelUrl = "https://localhost:7206/" + "customer/cart/index",

                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),

                // I followed the in class example but I was still getting errors in this area

                //{
                //    new Stripe.Checkout.SessionLineItemOptions
                //    {
                //        Price = "price_1MotwRLkdIwHu7ixYcPLm5uZ",
                //        Quantity = 2,
                //    },
                //},
                Mode = "payment",
            };

            // I followed the in class example but I was still getting errors in this area

            //foreach (var cartItem in shoppingCartVM.CartItems)
            //{
            //    var sessionLineItem = new SessionListLineItemsOptions
            //    {
            //        PriceData = new SessionLineItemPriceDataOptions
            //        {
            //            UnitAmount = (long)(cartItem.Book.Price * 100),
            //            Currency = "usd",
            //            ProductData = new SessionLineItemPriceDataProductDataOptions
            //            {
            //                Name = cartItem.Book.BookTitle
            //            }
            //        },
            //        Quantity = cartItem.Quantity

            //    };

            //    options.LineItems.Add(sessionLineItem)


            //}

            //var service = new Stripe.Checkout.SessionService();
            //Session session = service.Create(options);

            //shoppingCartVM.Order.SessionID = session.Id;
            //_dbContext.SaveChanges();

            //Response.Headers.Add

            return RedirectToAction("OrderConfirmation", new {id = shoppingCartVM.Order.OrderId});

        }

        public void UpdatePaymentStatus(int orderID, string sessionID, string paymentIntentID)
        {
            Order order = _dbContext.Orders.Find(orderID);

            if (!string.IsNullOrEmpty(sessionID))
            {
                order.SessionID = sessionID;

            }
            if (!string.IsNullOrEmpty(paymentIntentID))
            {
                order.PaymentIntentID = paymentIntentID;
                order.PaymentStatus = "Approved";
            }



        }

        public IActionResult OrderConfirmation(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Order order = _dbContext.Orders.Find(id);

            var sessID = order.SessionID;

            var service = new SessionService();

            Session session = service.Get(sessID);

            if(session.PaymentStatus.ToLower() == "paid")
            {
                order.PaymentIntentID = session.PaymentIntentId;

                order.PaymentStatus = "Approved";
            }

            List<Cart> userCartItems = _dbContext.Cart.ToList().Where(u => u.UserId ==userId).ToList();

            _dbContext.Cart.RemoveRange(userCartItems);
            _dbContext.SaveChanges();

            return View(id);
        }

    }
}
