using BooksApp_Spring2024.Data;
using BooksApp_Spring2024.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BooksApp_Spring2024.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,Employee")]
    public class CategoryController : Controller
    {

        private BooksDbContext _dbContext;

        public CategoryController(BooksDbContext booksDbContext)
        {
            _dbContext = booksDbContext; //assigns the context object to the private instance variable

        }


        public IActionResult Index() // list all categories
        {
            var listOfCategories = _dbContext.Categories.ToList();

            return View(listOfCategories);
        }

        //httpGet
        public IActionResult Create()//will allow the user to create a new category
        {

            return View();
        }


        [HttpPost]
        public IActionResult Create(Category categoryObj)
        {
            //test to male sure that category name is not test
            if (categoryObj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("name", "Category name cannot be 'test'");

            }

            //validation to make sure categoryName and description are not exactly the same
            if (categoryObj.Name.ToLower() == categoryObj.Description.ToLower())
            {
                ModelState.AddModelError("name", "Name cannot be the same as description");

            }

            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(categoryObj);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(categoryObj);

        }


        //httpGet
        public IActionResult Edit(int id)//will allow the user to edit a category
        {
            Category catObj = _dbContext.Categories.Find(id);

            return View(catObj);
        }


        [HttpPost]
        public IActionResult Edit(int id, [Bind("CategoryID, Name, Description")] Category categoryObj)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Update(categoryObj);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(categoryObj);

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Category categoryObj = _dbContext.Categories.Find(id);

            return View(categoryObj);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            Category categoryObj = _dbContext.Categories.Find(id);

            if (categoryObj != null)
            {
                _dbContext.Categories.Remove(categoryObj);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Category");

            }

            return View(categoryObj);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Category categoryObj = _dbContext.Categories.Find(id);
            return View(categoryObj);

        }
    }
}
