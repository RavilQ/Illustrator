using Illustration.Areas.AdminPanel.ViewModel;
using Illustration.DAL;
using Illustration.Models;
using Illustration.ViewModel;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

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

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            AppUser user = await _userManager.FindByEmailAsync(forgotPassword.Email);

            if (user == null)
            {
                return View("Error");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action("VerifyPasswordReset", "account", new { email = user.Email, token = token }, Request.Scheme);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("iillustrator@yandex.ru"));
            email.To.Add(MailboxAddress.Parse($"{user.Email}"));
            email.Subject = "Reset Your Password";
            email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>Hi.{user.Fullname} click <a href=\"{link}\">link</a> for the change your password</h1>" };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.yandex.com", 465, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate("iillustrator@yandex.ru", "illustrator123$");
            smtp.Send(email);
            smtp.Disconnect(true);

            //using var smtp = new SmtpClient();
            //smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            //smtp.Authenticate("della82@ethereal.email", "eQRmUMdy7eHWZwmArv");
            //smtp.Send(email);
            //smtp.Disconnect(true);

            return View();
        }

        public async Task<IActionResult> VerifyPasswordReset(string email, string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return View("Error");
            }

            if (!await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
            {
                return View("Error");
            }

            TempData["email"] = email;
            TempData["token"] = token;
            return RedirectToAction("ResetPassword");
        }

        public IActionResult ResetPassword()
        {
            var email = TempData["email"];
            var token = TempData["token"];

            ViewBag.email = email;
            ViewBag.token = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel resetVm)
        {
            AppUser user = await _userManager.FindByEmailAsync(resetVm.Email);

            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetVm.Token, resetVm.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("Password", err.Description);
                    return View();
                }
            }

            return RedirectToAction("login");

        }

    }
}
