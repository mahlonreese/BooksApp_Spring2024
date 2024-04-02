using BooksApp_Spring2024.Data;
using BooksApp_Spring2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksApp_Spring2024.Areas.Admin.Controllers
{
    [Area("admin")]

    public class OrderController : Controller
    {
        private BooksDbContext _dbContext;
        
        public OrderController(BooksDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Order> listOfOrders = _dbContext.Orders.Include(o => o.ApplicationUser);

            return View(listOfOrders);
        }
    }
}
