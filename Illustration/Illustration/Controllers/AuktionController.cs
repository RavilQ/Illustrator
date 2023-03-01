using Illustration.DAL;
using Illustration.Models;
using Illustration.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Data;

namespace Illustration.Controllers
{
    public class AuktionController : Controller
    {
        private readonly IllustratorDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public AuktionController(IllustratorDbContext context, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        public IActionResult Index(int id, string words)
        {
            var portrait = _context.Portraits
                .Include(x => x.PortraitImages)
                .Include(x=>x.PortraitCategories).ThenInclude(x=>x.Category)
                .Include(x=>x.PortraitTags).ThenInclude(x=>x.Tag).FirstOrDefault(x => x.Id == id);

            if (portrait == null)
            {
                return View("Error");
            }

            List<AppUser> users = new List<AppUser>();

            if (words != null)
            {
                users = _context.AppUsers.Where(x => x.UserName.Contains(words)).ToList();

            }
            else
            {
                users = _context.AppUsers.Where(x => x.HasMember == true).ToList();
            }

            AuktionViewModel viewmodel = new AuktionViewModel {

                Portrait = portrait,
                AppUsers = users,
                GroupMessages = _context.GroupMessages.Include(x=>x.AppUser).ToList()
            };

            return View(viewmodel);
        }

        public async Task<IActionResult> GroupMessageSend(string MyMessage)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (MyMessage.Length > 800)
            {
                return View();
            }

            GroupMessage message = new GroupMessage
            {

                AppUserId = user.Id,
                Text = MyMessage

            };
            ViewBag.signalruserimage = user.Image;
            _context.GroupMessages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
