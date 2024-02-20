using BooksApp_Spring2024.Data;
using BooksApp_Spring2024.Models;
using BooksApp_Spring2024.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksApp_Spring2024.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class BookController : Controller
    {

        private BooksDbContext _dbContext;
        private IWebHostEnvironment _environment; //allows us to fetch information about the server on which the project is running


        public BookController(BooksDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var listOfBooks = _dbContext.Books.ToList();

            return View(listOfBooks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> listOfCategories = _dbContext.Categories.ToList().Select(o =>
            new SelectListItem
            {
                Text = o.Name,
                Value = o.CategoryID.ToString()

                //known as transformation OR projection
            });


            //ViewBag.ListOfCategories = listOfCategories;

            //ViewData["ListOfCategoriesVD"] = listOfCategories;

            //ViewModel with a book and all the categories as IEnumerable of SelectListItems
            BookWithCategoriesVM bookWithCategoriesVM = new BookWithCategoriesVM();

            bookWithCategoriesVM.Book = new Book();

            bookWithCategoriesVM.ListOfCategories = listOfCategories;


            return View(bookWithCategoriesVM); //blank

        }

        [HttpPost]
        public IActionResult Create(BookWithCategoriesVM bookWithCategoriesVM, IFormFile imgFile)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _environment.WebRootPath;
                if (imgFile != null)
                {
                    //save the img file in the appropriate file

                    using (var fileStream = new FileStream(Path.Combine(wwwrootPath, @"images\bookImages\" + imgFile.FileName), FileMode.Create))
                    {

                        imgFile.CopyTo(fileStream); //saves the img file in the requested path/folder


                    }

                    bookWithCategoriesVM.Book.ImgUrl = @"\images\bookimages\" + imgFile.FileName;


                }


                _dbContext.Books.Add(bookWithCategoriesVM.Book);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Book");
            }

            return View(bookWithCategoriesVM); //if model is not valid, view displays the data that the user provided along with validation errors

        }


        [HttpGet]
        public IActionResult Edit(int id)//will allow the user to edit a category
        {
            var book = _dbContext.Books.Find(id);

            IEnumerable<SelectListItem> listOfCategories = _dbContext.Categories.ToList().Select(o =>
            new SelectListItem
            {
                Text = o.Name,
                Value = o.CategoryID.ToString()

                //known as transformation OR projection
            });


            BookWithCategoriesVM bookWithCategoriesVM = new BookWithCategoriesVM();

            bookWithCategoriesVM.Book = book;

            bookWithCategoriesVM.ListOfCategories = listOfCategories;


            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind("BookId,BookTitle, Author, Description, ISBN, Price, ImgUrl, CategoryID")]Book bookobj)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Books.Update(bookobj);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(bookobj);
        }

    }
}
