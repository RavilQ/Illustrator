using Illustration.DAL;
using Illustration.Models;
using Illustration.ViewModel;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using NuGet.Packaging.Signing;
using System.Data;
using MailKit.Net.Smtp;

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

            portrait.IsAuktion = true;
            _context.SaveChanges();

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

        public async Task<IActionResult> MakeOrder(int offerprice,string username)
        {
           
            var user = await _userManager.FindByNameAsync(username);

            var offer = _context.OfferPortraits.Include(x=>x.Portrait).FirstOrDefault(x => x.PortraitId == offerprice);

            var portrait = _context.Portraits.FirstOrDefault(x => x.Id == offer.PortraitId);
            

            Order order = new Order { 
            
                AppUserId = user.Id,
                PortraitId = offer.PortraitId,
                Status = Enum.OrderStatus.Accepted,
                Email = user.Email,
                Fullname = user.Fullname,
                Price = (int)offer.fivePercentPrice
            };

            var forbiddenorder = _context.Orders.FirstOrDefault(x => x.PortraitId == order.PortraitId);

            if (forbiddenorder!=null)
            {
                return Ok();
            }

            var messagelist = _context.GroupMessages.ToList();

            portrait.StockStatus = false;
            _context.Orders.Add(order);
            _context.GroupMessages.RemoveRange(messagelist);
            _context.SaveChanges();

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("iillustrator@yandex.ru"));
            email.To.Add(MailboxAddress.Parse($"{user.Email}"));
            email.Subject = "Congratulations!!";
            email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>You have purchased this masterpiece painting!!</h1>" };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.yandex.com", 465, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate("iillustrator@yandex.ru", "illustrator123$");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }

    }
}
