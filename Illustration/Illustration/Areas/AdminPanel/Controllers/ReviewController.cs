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
    public class ReviewController : Controller
    {
        private readonly IllustratorDbContext _context;

        public ReviewController(IllustratorDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? page = 1)
        {

            int pageSize = 5;

            var Reviews = _context.Reviews.Include(x => x.AppUser).ToList();
            Pagination<Review> paginatedList = new Pagination<Review>();

            ViewBag.Reviews = paginatedList.GetPagedNames(Reviews, page, pageSize);
            ViewBag.pageSize = pageSize;
            ViewBag.pageNumber = page;

            if (ViewBag.Reviews == null)
            {
                return View("Error");
            }

            return View();
        }

        public IActionResult Edit(int id)
        {
            var Reviews = _context.Reviews.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);

            return View(Reviews);
        }

        public IActionResult Approve(int id)
        {
            var Reviews = _context.Reviews.FirstOrDefault(x => x.Id == id);

            if (Reviews == null)
            {
                return View("Error");
            }

            Reviews.Status = Enum.OrderStatus.Accepted;
            _context.SaveChanges();

            return RedirectToAction("Index", "Review");
        }

        public IActionResult Reject(int id)
        {
            var Reviews = _context.Reviews.FirstOrDefault(x => x.Id == id);

            if (Reviews == null)
            {
                return View("Error");
            }

            Reviews.Status = Enum.OrderStatus.Rejected;
            _context.SaveChanges();

            return RedirectToAction("Index", "Review");
        }
    }
}
