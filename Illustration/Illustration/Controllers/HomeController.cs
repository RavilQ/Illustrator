using Illustration.DAL;
using Illustration.Models;
using Illustration.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Illustration.Controllers
{
    public class HomeController : Controller
    {
        private readonly IllustratorDbContext _context;

        public HomeController(IllustratorDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var images = _context.Sliders.Take(3).ToList();
            var portraits = _context.Portraits
                .Include(x => x.PortraitImages)
                .Include(x => x.PortraitCategories).ThenInclude(x => x.Category)
                .Include(x => x.PortraitTags).ThenInclude(x => x.Tag).Where(x => x.IsSpecial == true);

            HomeViewModel model = new HomeViewModel {

                Sliders = images,
                Portraits = portraits.Take(8).ToList(),
                SecondPortraits = portraits.Skip(8).Take(8).ToList()

            };

            return View(model);
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }
    }
}