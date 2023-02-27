using Illustration.DAL;
using Illustration.Models;
using Illustration.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Index(int id)
        {
            var portrait = _context.Portraits
                .Include(x => x.PortraitImages)
                .Include(x=>x.PortraitCategories).ThenInclude(x=>x.Category)
                .Include(x=>x.PortraitTags).ThenInclude(x=>x.Tag).FirstOrDefault(x => x.Id == id);

            if (portrait == null)
            {
                return View("Error");
            }

            AuktionViewModel viewmodel = new AuktionViewModel {

                Portrait = portrait,
                AppUsers = _context.AppUsers.Where(x=>x.HasMember==true).ToList()

            };

            return View(viewmodel);
        }
    }
}
