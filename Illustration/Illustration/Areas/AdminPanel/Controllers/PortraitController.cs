using ClosedXML.Excel;
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
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class PortraitController : Controller
    {
        private readonly IllustratorDbContext _context;

        public PortraitController(IllustratorDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page = 1, string? search = null)
        {
            int pageSize = 5;

            List<Portrait> portraits = new List<Portrait>();

            if (search!=null)
            {
                portraits = _context.Portraits.Include(x => x.PortraitImages).Where(x=>x.Name.Contains(search)).ToList();
            }
            else
            {
                portraits = _context.Portraits.Include(x => x.PortraitImages).ToList();
            }
            
            Pagination<Portrait> paginatedList = new Pagination<Portrait>();

            ViewBag.portrait = paginatedList.GetPagedNames(portraits, page, pageSize);
            ViewBag.pageSize = pageSize;
            ViewBag.pageNumber = page;
            ViewBag.search = search;

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
