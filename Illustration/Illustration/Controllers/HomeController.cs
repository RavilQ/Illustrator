using Illustration.DAL;
using Illustration.Models;
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

            return View(images);
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