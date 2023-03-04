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
    public class UserController : Controller
    {
        private readonly IllustratorDbContext _context;

        public UserController(IllustratorDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page = 1)
        {
            int pageSize = 5;

            var user = _context.AppUsers.ToList();
            Pagination<AppUser> paginatedList = new Pagination<AppUser>();

            ViewBag.user = paginatedList.GetPagedNames(user, page, pageSize);
            ViewBag.pageSize = pageSize;
            ViewBag.pageNumber = page;

            if (ViewBag.user == null)
            {
                return View("Error");
            }

            return View();
        }
    }
}
