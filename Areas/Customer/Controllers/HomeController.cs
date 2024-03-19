using BooksApp_Spring2024.Data;
using BooksApp_Spring2024.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace BooksApp_Spring2024.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private BooksDbContext _dbContext;

        private readonly ILogger<HomeController> _logger;

        //constructor method
        public HomeController(ILogger<HomeController> logger, BooksDbContext dbContext)
        {
            _logger = logger;

            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var books = _dbContext.Books.Include(b => b.category);

            return View(books.ToList());
        }

        public IActionResult Details(int id)
        {

            Book book = _dbContext.Books.Find(id);

            //Along with the book, the category is loaded as well
            _dbContext.Entry(book).Reference(b => b.category).Load();

            var cart = new Cart
            {
                BookId = id,
                Book = book,

                Quantity = 1

            };

            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(Cart cart)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cart.UserId = userId; //plugged in the userId into the cart object

            _dbContext.Cart.Add(cart);//adding a new record into the cart DbSet

            _dbContext.SaveChanges();

            return RedirectToAction("Index");

        }


        public IActionResult Privacy()
        {
            //returns the view related to the privacy view
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}