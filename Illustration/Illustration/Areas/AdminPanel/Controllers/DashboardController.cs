using Illustration.Areas.AdminPanel.ViewModel;
using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Illustration.Area.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DashboardController : Controller
    {
        private readonly IllustratorDbContext _context;

        public DashboardController(IllustratorDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Todolist = _context.TodoLists.ToList();

            var query = _context.Portraits.Include(x => x.PortraitCategories).ThenInclude(x => x.Category);

            List<Category> list = new List<Category>();

            foreach (var item in query)
            {
                foreach (var item2 in item.PortraitCategories)
                {
                    list.Add(item2.Category);
                }
            }

            DashboardVIewModel viewmodel = new DashboardVIewModel {
                Messages = _context.ContactMessages
    .Include(x => x.AppUser)
    .GroupBy(x => x.AppUserId)
    .Select(g => g.OrderByDescending(x => x.CreatAt).FirstOrDefault())
    .ToList(),
            Portraits = _context.Portraits.ToList(),
                Orders = _context.Orders.ToList(),
                Categories = list
            };

            

            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult Index(TodoList list)
        {
            if (list.Text.Length>18)
            {
                return RedirectToAction("Index");
            }

            _context.TodoLists.Add(list);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult TodolistDelete(int id)
        {
            var list = _context.TodoLists.FirstOrDefault(x => x.Id == id);

            if (list == null)
            {
                return View("Index");
            }

            _context.Remove(list);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //public IActionResult SearchingPanel(int? page=1, string? search = null)
        //{
        //    int pageSize = 5;

        //    var portraits = _context.Portraits.Include(x => x.PortraitImages).Where(x=>x.Name.Contains(search)).ToList();
        //    Pagination<Portrait> paginatedList = new Pagination<Portrait>();

        //    ViewBag.portrait = paginatedList.GetPagedNames(portraits, page, pageSize);
        //    ViewBag.pageSize = pageSize;
        //    ViewBag.pageNumber = page;

        //    if (ViewBag.portrait == null)
        //    {
        //        return View("Error");
        //    }

        //    return View();
        //}

        public IActionResult SearchingPanel(int? page = 1, string? search = null)
        {
            if (search == null)
            {
                return RedirectToAction("Index");
            }

            AdminSearch serching = new AdminSearch { 
            
                Admins = _context.AppUsers.Where(x=>x.RoleName=="Admin" && x.UserName.Contains(search)).ToList(),
                Users = _context.AppUsers.Where(x => x.HasMember == true && x.UserName.Contains(search)).ToList(),
                Portraits = _context.Portraits.Where(x => x.Name.Contains(search)).ToList(),
                Categorys = _context.Categories.Where(x =>x.Name.Contains(search)).ToList(),
                Tags = _context.Tags.Where(x =>x.Name.Contains(search)).ToList(),
            };

            ViewBag.search = search;

            return View(serching);
        }
    }
}
