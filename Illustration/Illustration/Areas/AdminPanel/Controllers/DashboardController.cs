using Illustration.Areas.AdminPanel.ViewModel;
using Illustration.DAL;
using Illustration.Enum;
using Illustration.Helper;
using Illustration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

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

            var query = _context.Portraits.Include(x => x.PortraitCategories).ThenInclude(x => x.Category).Include(x=>x.PortraitTags).ThenInclude(x=>x.Tag);

            var orders = _context.Orders.Include(x => x.Portrait).ThenInclude(x => x.PortraitTags).ToList();

            var totalPrice = orders.Sum(order => order.Price);

            List<Portrait> lsted = new List<Portrait>();

            ViewBag.totalprice = totalPrice;

            foreach (var item in orders)
            {
                var ptest = _context.Portraits.Include(x => x.PortraitTags).ThenInclude(x => x.Tag).FirstOrDefault(x=>x.Id==item.Portrait.Id);
                lsted.Add(ptest);
            }

            List<PortraitTag> ptags1 = new List<PortraitTag>();
            List<PortraitTag> ptags2 = new List<PortraitTag>();
            List<PortraitTag> ptags3 = new List<PortraitTag>();
            List<PortraitTag> ptags4 = new List<PortraitTag>();
            List<PortraitTag> ptags5 = new List<PortraitTag>();

            foreach (var item in lsted)
            {
                ptags1.AddRange(item.PortraitTags.Where(x => x.Tag.Id == 1));
                ptags2.AddRange(item.PortraitTags.Where(x => x.Tag.Id == 2));
                ptags3.AddRange(item.PortraitTags.Where(x => x.Tag.Id == 3));
                ptags4.AddRange(item.PortraitTags.Where(x => x.Tag.Id == 4));
                ptags5.AddRange(item.PortraitTags.Where(x => x.Tag.Id == 5));
            }

            ViewBag.list1 = ptags1.Count;
            ViewBag.list2 = ptags2.Count;
            ViewBag.list3 = ptags3.Count;
            ViewBag.list4 = ptags4.Count;
            ViewBag.list5 = ptags5.Count;

            List<Category> list = new List<Category>();

            List<Tag> tlist = new List<Tag>();

            foreach (var item in query)
            {
                foreach (var item2 in item.PortraitCategories)
                {
                    list.Add(item2.Category);
                }

                foreach (var item3 in item.PortraitTags)
                {
                    tlist.Add(item3.Tag);
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
                Categories = list,
                Tags = tlist
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
