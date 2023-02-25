using Illustration.DAL;
using Illustration.Models;
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

            return View(portrait);
        }
    }
}
