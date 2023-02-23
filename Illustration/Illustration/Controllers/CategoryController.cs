using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Illustration.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "SuperAdmin")]
    public class CategoryController : Controller
    {
        private readonly IllustratorDbContext _context;

        public CategoryController(IllustratorDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page = 1)
        {
            int pageSize = 5;

            var category = _context.Categories.ToList();
            Pagination<Category> paginatedList = new Pagination<Category>();

            ViewBag.category = paginatedList.GetPagedNames(category, page, pageSize);
            ViewBag.pageSize = pageSize;
            ViewBag.pageNumber = page;

            if (ViewBag.category == null)
            {
                return View("Error");
            }

            return View();
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category == null)
            {
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_context.Categories.Any(x => x.Name == category.Name))
            {
                ModelState.AddModelError("Name", "You dont create Category with the same name !!");

                return View();
            }

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {


            var category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return View("Error");
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category == null)
            {
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var oldCategory = _context.Categories.FirstOrDefault(x => x.Id == category.Id);

            if (_context.Categories.Any(x => x.Name == category.Name))
            {
                ModelState.AddModelError("Name", "You dont create Category with the same name !!");

                return View();
            }

            oldCategory.Name = category.Name;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return View("Error");
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
