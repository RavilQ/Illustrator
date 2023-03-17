using ClosedXML.Excel;
using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Illustration.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CategoryController : Controller
    {
        private readonly IllustratorDbContext _context;

        public CategoryController(IllustratorDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page = 1, string? search = null)
        {
            int pageSize = 5;

            List<Category> category = new List<Category>();

            if (search != null)
            {
                category = _context.Categories.Where(x=>x.Name.Contains(search)).ToList();
            }
            else
            {
                category = _context.Categories.ToList();
            }

            Pagination<Category> paginatedList = new Pagination<Category>();

            ViewBag.category = paginatedList.GetPagedNames(category, page, pageSize);
            ViewBag.pageSize = pageSize;
            ViewBag.pageNumber = page;
            ViewBag.search = search;

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

        public IActionResult ExportAsExcell()
        {
            var category = _context.Categories.AsQueryable();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ForExel.ToDataTable<Category>(category.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.UtcNow.AddHours(4).Date}-houses.xlsx");
                }
            }

            return View();
        }
    }
}
