using BooksApp_Spring2024.Data;
using BooksApp_Spring2024.Models;
using BooksApp_Spring2024.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksApp_Spring2024.Areas.Admin.Controllers
{
    [Area("admin")]

    public class OrderController : Controller
    {
        private BooksDbContext _dbContext;

        [BindProperty]
        public OrderVM orderVM { get; set; }

        public OrderController(BooksDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Order> listOfOrders = _dbContext.Orders.Include(o => o.ApplicationUser);

            return View(listOfOrders);
        }

        public IActionResult Details(int id)
        {
            Order order = _dbContext.Orders.Find(id);

            _dbContext.Entry(order).Reference(o => o.ApplicationUser).Load();

            IEnumerable<OrderDetail> orderDetails = _dbContext.OrderDetails.Where(od => od.OrderId == id).Include(od => od.Book);

            OrderVM orderVM = new OrderVM
            {
                Order = order,
                OrderDetails = orderDetails
            }
            ;

            return View(orderVM);

        }

        [HttpPost]
        public IActionResult UpdateOrderInformation(int id) 
        {
            Order orderFromDB = _dbContext.Orders.Find(orderVM.Order.OrderId);

            orderFromDB.Name = orderVM.Order.Name; //puts the value from the view into the object from the database

            orderFromDB.StreetAddress = orderVM.Order.StreetAddress;

            orderFromDB.City = orderVM.Order.City;
            orderFromDB.State = orderVM.Order.State;
            orderFromDB.PostalCode = orderVM.Order.PostalCode;
            orderFromDB.Phone = orderVM.Order.Phone;

            if( !string.IsNullOrEmpty(orderVM.Order.Carrier))
                orderFromDB.Carrier = orderVM.Order.Carrier;
            
            if (!string.IsNullOrEmpty(orderVM.Order.ShippingDate.ToString()))
                orderFromDB.Carrier = orderVM.Order.ShippingDate.ToString();

            if (!string.IsNullOrEmpty(orderVM.Order.TrackingNumber))
                orderFromDB.Carrier = orderVM.Order.TrackingNumber;

            _dbContext.Orders.Update(orderFromDB);
            _dbContext.SaveChanges();

            return RedirectToAction("Details", new {id = orderFromDB.OrderId});

        }
    }
}
