using Illustration.Areas.AdminPanel.ViewModel;
using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Illustration.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IllustratorDbContext _context;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IllustratorDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> Index(int? page = 1)
        {
            int pageSize = 5;

            List<AppUser> admins = _context.AppUsers.Where(x => x.HasMember == false && x.RoleName!="SuperAdmin").ToList();
            var users = _userManager.Users.ToList();
            Pagination<AppUser> paginatedList = new Pagination<AppUser>();
            ViewBag.message = paginatedList.GetPagedNames(admins, page, pageSize);

            if (ViewBag.message == null)
            {
                return View("Error");
            }

            return View();
        }

        public IActionResult Delete(string id)
        {
            var admin = _context.AppUsers.FirstOrDefault(x => x.Id == id);

            if (admin == null)
            {
                return View("Error");
            }

            _context.AppUsers.Remove(admin);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCreateViewModel newadmin)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await _userManager.FindByNameAsync(newadmin.Username) != null)
            {
                ModelState.AddModelError("Username", "That Username alreaady taken!!");
            }

            AppUser admin = new AppUser
            {
                UserName = newadmin.Username,
                Fullname = newadmin.Fullname,
                HasMember = false
            };

            var result = await _userManager.CreateAsync(admin, newadmin.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }

            await _userManager.AddToRoleAsync(admin, "Admin");

            var adminf = await _userManager.FindByNameAsync(admin.UserName);

            var user = _context.AppUsers.FirstOrDefault(x => x.Id == adminf.Id);

            user.RoleName = "Admin";
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
