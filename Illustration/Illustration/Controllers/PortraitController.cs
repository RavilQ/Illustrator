using Illustration.DAL;
using Illustration.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Illustration.Controllers
{
    public class PortraitController : Controller
    {
        private readonly IllustratorDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PortraitController(IllustratorDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Detail(int id)
        {
            var portrait = _context.Portraits
                .Include(x => x.PortraitImages)
                .Include(x => x.PortraitCategories)
                .Include(x => x.PortraitTags).FirstOrDefault(x => x.Id == id);

            DetailViewModel model = new DetailViewModel {

                Portrait = portrait,
                Portraits = _context.Portraits.Include(x => x.PortraitImages)
                .Include(x => x.PortraitCategories).ThenInclude(x=>x.Category)
                .Include(x => x.PortraitTags).ThenInclude(x=>x.Tag).Take(4).Where(x=>x.Id != id).ToList()

            };

            return View(model);
        }
    }
}
