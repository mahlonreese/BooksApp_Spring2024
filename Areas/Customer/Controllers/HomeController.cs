using BooksApp_Spring2024.Data;
using BooksApp_Spring2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

            return View(book);
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