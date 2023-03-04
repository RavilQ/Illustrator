using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Illustration.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "SuperAdmin")]
    public class PortraitController : Controller
    {
        private readonly IllustratorDbContext _context;

        public PortraitController(IllustratorDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page = 1)
        {
            int pageSize = 5;

            var portraits = _context.Portraits.Include(x=>x.PortraitImages).ToList();
            Pagination<Portrait> paginatedList = new Pagination<Portrait>();

            ViewBag.portrait = paginatedList.GetPagedNames(portraits, page, pageSize);
            ViewBag.pageSize = pageSize;
            ViewBag.pageNumber = page;

            if (ViewBag.portrait == null)
            {
                return View("Error");
            }

            return View();
        }

        public IActionResult Special(int id)
        {
            var portrait = _context.Portraits.FirstOrDefault(x => x.Id == id);

            if (portrait == null)
            {
                return View("Error");
            }

            portrait.IsSpecial = true;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Simple(int id)
        {
            var portrait = _context.Portraits.FirstOrDefault(x => x.Id == id);

            if (portrait == null)
            {
                return View("Error");
            }

            portrait.IsSpecial = false;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
