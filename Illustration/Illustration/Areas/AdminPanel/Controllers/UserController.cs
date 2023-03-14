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
    public class UserController : Controller
    {
        private readonly IllustratorDbContext _context;

        public UserController(IllustratorDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page = 1, string? search=null)
        {
            int pageSize = 5;

            List<AppUser> user = new List<AppUser>();

            if (search!=null)
            {
                user = _context.AppUsers.Where(x=>x.HasMember==true && x.UserName.Contains(search)).ToList();
            }
            else
            {
                user = _context.AppUsers.Where(x => x.HasMember == true).ToList();
            }

            Pagination<AppUser> paginatedList = new Pagination<AppUser>();

            ViewBag.user = paginatedList.GetPagedNames(user, page, pageSize);
            ViewBag.pageSize = pageSize;
            ViewBag.pageNumber = page;
            ViewBag.search = search;

            if (ViewBag.user == null)
            {
                return View("Error");
            }

            return View();
        }

        public IActionResult BanPeople(string id)
        {
            var user = _context.AppUsers.FirstOrDefault(x => x.Id == id);

            if (user==null)
            {
                return View("Error");
            }

            user.IsBan = true;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult UnbanPeople(string id)
        {
            var user = _context.AppUsers.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return View("Error");
            }

            user.IsBan = false;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
