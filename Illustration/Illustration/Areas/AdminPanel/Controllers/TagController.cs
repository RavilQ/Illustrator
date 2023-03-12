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
    public class TagController : Controller
    {
        private readonly IllustratorDbContext _context;

        public TagController(IllustratorDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page = 1,string? search=null)
        {

            int pageSize = 5;

            List<Tag> tag = new List<Tag>();

            if (search!=null)
            {
                tag = _context.Tags.Where(x=>x.Name.Contains(search)).ToList();
            }
            else
            {
                tag = _context.Tags.ToList();
            }

            if (tag.Count % 5 == 0)
            {
                page=1;
            }
            Pagination<Tag> paginatedList = new Pagination<Tag>();

            ViewBag.tag = paginatedList.GetPagedNames(tag, page, pageSize);
            ViewBag.pageSize = pageSize;
            ViewBag.pageNumber = page;
            ViewBag.search = search;

            if (ViewBag.tag == null)
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
        public IActionResult Create(Tag tag)
        {
            if (tag == null)
            {
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_context.Tags.Any(x => x.Name == tag.Name))
            {
                ModelState.AddModelError("Name", "You dont create Tag with the same name !!");

                return View();
            }

            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {


            var tag = _context.Tags.FirstOrDefault(x => x.Id == id);

            if (tag == null)
            {
                return View("Error");
            }

            return View(tag);
        }
        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            if (tag == null)
            {
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var oldTag = _context.Tags.FirstOrDefault(x => x.Id == tag.Id);

            if (_context.Tags.Any(x => x.Name == tag.Name))
            {
                ModelState.AddModelError("Name", "You dont create Tag with the same name !!");

                return View();
            }

            oldTag.Name = tag.Name;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var tag = _context.Tags.FirstOrDefault(x => x.Id == id);

            if (tag == null)
            {
                return View("Error");
            }

            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
