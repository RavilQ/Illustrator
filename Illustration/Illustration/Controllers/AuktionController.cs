using Illustration.DAL;
using Illustration.Models;
using Illustration.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Data;

namespace Illustration.Controllers
{
    public class AuktionController : Controller
    {
        private readonly IllustratorDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public AuktionController(IllustratorDbContext context, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id, decimal Offerprice)
        {
            var portrait = _context.Portraits
                .Include(x => x.PortraitImages)
                .Include(x=>x.PortraitCategories).ThenInclude(x=>x.Category)
                .Include(x=>x.PortraitTags).ThenInclude(x=>x.Tag).FirstOrDefault(x => x.Id == id);

            if (portrait == null)
            {
                return View("Error");
            }

            List<AppUser> users = _context.AppUsers.Where(x => x.HasMember == true).ToList();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (Offerprice!=0)
            {
                OfferPortrait offer = new OfferPortrait
                {

                    fivePercentPrice = Offerprice,
                    PortraitId = portrait.Id,
                    AppUserId = user.Id

                };

                var repeat = _context.OfferPortraits.FirstOrDefault(x => x.PortraitId == offer.PortraitId);

                if (repeat != null)
                {
                    repeat.fivePercentPrice = Offerprice;
                    _context.SaveChanges();
                }
                else
                {
                    _context.OfferPortraits.Add(offer);
                    _context.SaveChanges();
                }
              
            }
            AuktionViewModel viewmodel = new AuktionViewModel {

                Portrait = portrait,
                AppUsers = users,
                GroupMessages = _context.GroupMessages.Include(x => x.AppUser).ToList(),
                Offer = _context.OfferPortraits.FirstOrDefault(x => x.PortraitId == portrait.Id)
            };
            return View(viewmodel);
        }

        public async Task<IActionResult> GroupMessageSend(string MyMessage)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (MyMessage.Length > 800)
            {
                return View();
            }

            GroupMessage message = new GroupMessage
            {

                AppUserId = user.Id,
                Text = MyMessage

            };
            ViewBag.signalruserimage = user.Image;
            _context.GroupMessages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Auktionoffer(int id)
        {
            OfferPortrait offer = _context.OfferPortraits.FirstOrDefault(x => x.PortraitId == id);

            decimal percent = 0;

            if (offer == null)
            {
                percent = 0;
            }
            else {
                percent = offer.fivePercentPrice;
            }
           
            return Json(percent);
        }

    }
}
