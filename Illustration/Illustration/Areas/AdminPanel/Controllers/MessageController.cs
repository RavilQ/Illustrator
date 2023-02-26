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
    public class MessageController : Controller
    {
        private readonly IllustratorDbContext _context;

        public MessageController(IllustratorDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page = 1)
        {
            int pageSize = 5;

            var messages = _context.ContactMessages.Include(x=>x.AppUser).Where(x=>x.IsMember==true).ToList();
            Pagination<ContactMessage> paginatedList = new Pagination<ContactMessage>();

            ViewBag.message = paginatedList.GetPagedNames(messages, page, pageSize);

            if (ViewBag.message == null)
            {
                return View("Error");
            }

            return View();
        }

        public IActionResult Answer(string AppUserId)
        {
            ViewBag.id = AppUserId;
            return View();
        }

        [HttpPost]
        public IActionResult Answer(ContactMessage message)
        {
            if (message == null)
            {
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            message.IsMember = false;
            message.Name = "Admin";

            _context.ContactMessages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("Index", "Message");
        }

        public IActionResult Delete(int Id)
        {
            var message = _context.ContactMessages.FirstOrDefault(x => x.Id == Id);

            if (message == null)
            {
                return View("Error");
            }

            _context.ContactMessages.Remove(message);
            _context.SaveChanges();
            return RedirectToAction("Index", "Message");
        }
    }
}
