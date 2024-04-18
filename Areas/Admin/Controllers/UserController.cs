using BooksApp_Spring2024.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BooksApp_Spring2024.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        private BooksDbContext _dbcontext;

        private UserManager<IdentityUser> _userManager;

        public UserController(BooksDbContext dbcontext, UserManager<IdentityUser> userManager)
        {
            _dbcontext = dbcontext;

            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<ApplicationUser> userList = _dbcontext.ApplicationUsers.ToList();

            //fetches roles
            var allRoles = _dbcontext.Roles.ToList();
            //fetches aspnetUserRoles
            var userRoles = _dbcontext.UserRoles.ToList();

            foreach (var user in userList)
            {
                var roleId = userRoles.Find(r => r.UserId ==  user.Id).RoleId;

                var roleName = allRoles.Find(r => r.Id == roleId).Name;

                user.RoleName = roleName;
            }

            return View(userList);
        }

        public IActionResult LockUnlock(string id)
        {
            var userFromDB = _dbcontext.ApplicationUsers.Find(id);

            if (userFromDB.LockoutEnd != null && userFromDB.LockoutEnd > DateTime.Now)
            {
                //the user is locked, we can go ahead and unlock account
                userFromDB.LockoutEnd = DateTime.Now;

            }
            else
            {
                //user is unlocked, we can lock their account
                userFromDB.LockoutEnd = DateTime.Now.AddYears(10);
            }

            _dbcontext.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult EditUserRole(string id)
        {
            var currentUserRole = _dbcontext.UserRoles.FirstOrDefault(ur => ur.UserId == id);

            IEnumerable<SelectListItem> listOfRoles = _dbcontext.Roles.ToList().Select(r =>
            new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()

                //known as transformation OR projection
            });

            ViewBag.ListOfRoles = listOfRoles;

            ViewBag.UserInfo = _dbcontext.ApplicationUsers.Find(id);

            return View(currentUserRole);

        }

        [HttpPost]
        public IActionResult EditUserRole(Microsoft.AspNetCore.Identity.IdentityUserRole<string> updateRole)
        {
            ApplicationUser applicationUser = _dbcontext.ApplicationUsers.Find(updateRole.UserId);

            string newRoleName = _dbcontext.Roles.Find(updateRole.RoleId).Name;

            string oldRoleId = _dbcontext.UserRoles.FirstOrDefault(u => u.UserId == applicationUser.Id).RoleId;

            string oldRoleName = _dbcontext.Roles.Find(oldRoleId).Name;

            _userManager.RemoveFromRoleAsync(applicationUser, oldRoleName).GetAwaiter().GetResult();

            _userManager.AddToRoleAsync(applicationUser, newRoleName).GetAwaiter().GetResult();

            return RedirectToAction("Index");

        }

    }
}
