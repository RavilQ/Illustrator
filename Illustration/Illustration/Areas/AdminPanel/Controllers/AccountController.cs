using Illustration.Areas.AdminPanel.ViewModel;
using Illustration.DAL;
using Illustration.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Illustration.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IllustratorDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IllustratorDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> CreateAdmin()
        {
            AppUser admin = new AppUser
            {
                UserName = "Sun",
                Fullname = "Celil Ehmedov",
                HasMember = false
            };

            var result = await _userManager.CreateAsync(admin, "Ravil123$");

            await _userManager.AddToRoleAsync(admin, "SuperAdmin");

            return Ok(result);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel loginVm)
        {
            AppUser user = await _userManager.FindByNameAsync(loginVm.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is wrong !!");
                return View();
            }

            //var roles = await _userManager.GetRolesAsync(user);

            //if (!roles.Contains("SuperAdmin") && !roles.Contains("Admin") && !roles.Contains("Editor"))
            //{
            //    ModelState.AddModelError("", "Username or Password is wrong !!");
            //    return View();
            //}

            var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, false, false);


            if (!result.Succeeded)
            {
                ModelState.AddModelError("Password", "Password is wrong !!");
                return View();
            }

            if (user.RoleName=="Admin" && user.PasswordResetCheck==false)
            {
                return RedirectToAction("AdminPasswordReset",new { password = loginVm.Password});
            }

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        public IActionResult AdminPasswordReset(string password)
        {
            ViewBag.user = password;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminPasswordReset(AdminResetPasswordViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var newuser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (viewmodel.Password != null && viewmodel.ConfirmPassword != null)
            {
                    var netice = await _userManager.ChangePasswordAsync(newuser, viewmodel.user, viewmodel.ConfirmPassword);
                    if (netice == null)
                    {
                        return View("Error");
                    }
            }

            newuser.PasswordResetCheck = true;
            _context.SaveChanges();

            return RedirectToAction("Index", "Dashboard");
        }

    }
}
