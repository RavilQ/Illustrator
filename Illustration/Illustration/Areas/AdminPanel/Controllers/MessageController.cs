﻿using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

namespace Illustration.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "SuperAdmin,Admin")]
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
            List<AppUser> users = _context.AppUsers.Where(x=>x.HasMember==true).ToList();
            //foreach (var item in messages)
            //{
            //  users = _context.AppUsers.Where(x => x.Id == item.AppUserId).ToList();
            //}
            ViewBag.userss = messages;
            Pagination<AppUser> paginatedList = new Pagination<AppUser>();
            ViewBag.message = paginatedList.GetPagedNames(users, page, pageSize);

            if (ViewBag.message == null)
            {
                return View("Error");
            }

            return View();
        }

        public IActionResult Answer(string AppUserId)
        {
            ViewBag.id = AppUserId;
            ViewBag.messages = _context.ContactMessages.Include(x=>x.AppUser).Where(x => x.AppUserId == AppUserId).ToList();
            return View();
        }

        public IActionResult AdminAnswer(string MyMessage,string AppUserId)
        {
            if (MyMessage == null)
            {
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            ContactMessage message = new ContactMessage { 
                IsMember = false,
                Name = "Admin",
                Text = MyMessage,
                AppUserId = AppUserId
            };

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
