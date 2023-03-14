using Illustration.DAL;
using Illustration.Models;
using Illustration.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Illustration.Controllers
{
    public class HomeController : Controller
    {
        private readonly IllustratorDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(IllustratorDbContext context, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var images = _context.Sliders.OrderBy(x=>x.Waitlist).ToList();
            var portraits = _context.Portraits
                .Include(x => x.PortraitImages)
                .Include(x => x.PortraitCategories).ThenInclude(x => x.Category)
                .Include(x => x.PortraitTags).ThenInclude(x => x.Tag).Where(x => x.IsSpecial == true);

            var settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value);

            HomeViewModel model = new HomeViewModel {

                Sliders = images,
                Portraits = portraits.Take(8).ToList(),
                SecondPortraits = portraits.Skip(8).Take(8).ToList(),
                Settings = settings

            };

            return View(model);
        }

        public IActionResult AboutUs()
        {
            var settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value);

            AboutUsVIewModel viewmodel = new AboutUsVIewModel { 
            
                Settings = settings,
                Creators = _context.Creators.Take(4).ToList()

            };

            return View(viewmodel);
        }

        public IActionResult Blog()
        {
            return View();
        }

        public async Task<IActionResult> NewSubscriber()
        {
            AppUser user = null;

            if (User.Identity.IsAuthenticated)
            {
               user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            else
            {
                return RedirectToAction("Blog");
            }

            if (user.Email==null || _context.Subscribers.Any(x=>x.AppUserId==user.Id))
            {
                return RedirectToAction("Blog");
            }

            Subscriber subscribe = new Subscriber { 
            
                AppUserId = user.Id,
                Email = user.Email

            };

            _context.Subscribers.Add(subscribe);
            _context.SaveChanges();

            return RedirectToAction("Blog");
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactMessage message)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = null;

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            else
            {
                return View();
            }

            if (user!=null)
            {
                message.AppUserId = user.Id;
            }

            message.IsMember = true;

            _context.ContactMessages.Add(message);
            _context.SaveChanges();

            return View();
        }
    }
}