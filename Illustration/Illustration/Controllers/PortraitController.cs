using Illustration.DAL;
using Illustration.Models;
using Illustration.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Illustration.Controllers
{
    public class PortraitController : Controller
    {
        private readonly IllustratorDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public PortraitController(IllustratorDbContext context, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        public IActionResult Detail(int id)
        {
            var portrait = _context.Portraits
                .Include(x => x.PortraitImages)
                .Include(x => x.PortraitCategories)
                .Include(x => x.PortraitTags).FirstOrDefault(x => x.Id == id);

            DetailViewModel model = new DetailViewModel {

                Portrait = portrait,
                Portraits = _context.Portraits.Include(x => x.PortraitImages)
                .Include(x => x.PortraitCategories).ThenInclude(x=>x.Category)
                .Include(x => x.PortraitTags).ThenInclude(x=>x.Tag).Where(x=>x.Id != id && x.IsSpecial==true).Take(4).ToList()

            };

            return View(model);
        }

        public async Task<IActionResult> AddToWishList(int id)
        {
            AppUser user = null;

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            List<WishListItem> wishListItem = new List<WishListItem>();

            if (!_context.Portraits.Any(x => x.Id == id))
            {
                return View("Error");
            }

           
            if (user != null)
            {
                WishListItem item = _context.WishListItems.FirstOrDefault(x => x.PortraitId == id && x.AppUserId == user.Id);

                if (item == null)
                {
                    item = new WishListItem
                    {
                        AppUserId = user.Id,
                        PortraitId = id
                    };

                    _context.WishListItems.Add(item);
                }

                _context.SaveChanges();

                wishListItem = _context.WishListItems.Include(x => x.Portrait).ThenInclude(x => x.PortraitImages).ToList();


            }
            else
            {
                var wishListStr = HttpContext.Request.Cookies["wishListITem"];
                List<WishListCookieItemViewModel> cookieItems = null;

                if (wishListStr == null)
                {
                    cookieItems = new List<WishListCookieItemViewModel>();
                }
                else
                {
                    cookieItems = JsonConvert.DeserializeObject<List<WishListCookieItemViewModel>>(wishListStr);
                }

                WishListCookieItemViewModel cookieItem = cookieItems.FirstOrDefault(x => x.PortraitId == id);

                if (cookieItem == null)
                {
                    cookieItem = new WishListCookieItemViewModel
                    {

                        PortraitId = id
                    };

                    cookieItems.Add(cookieItem);


                }
                else
                {

                }

                var jsonStr = JsonConvert.SerializeObject(cookieItems);

                HttpContext.Response.Cookies.Append("wishListItem", jsonStr);

                WishListItem item = new WishListItem();

                foreach (var items in cookieItems)
                {
                    Portrait portrait = _context.Portraits.Include(x => x.PortraitImages).FirstOrDefault(x => x.Id == items.PortraitId);

                    item = new WishListItem
                    {
                        Portrait = portrait
                    };

                    wishListItem.Add(item);
                }


            }
            return PartialView("_wishListPartial", wishListItem);
        }

        public async Task<IActionResult> DeleteToWishList(int id)
        {
            AppUser user = null;

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);

            }

            List<WishListItem> wishListItems = new List<WishListItem>();



            if (user != null)
            {
                var wishListItem = _context.WishListItems.FirstOrDefault(x => x.Id == id && x.AppUserId == user.Id );
                if (wishListItem == null)
                {
                    return View("Error");
                }
                _context.WishListItems.Remove(wishListItem);
                _context.SaveChanges();

                wishListItems = _context.WishListItems.Include(x => x.Portrait).ThenInclude(x => x.PortraitImages).ToList();
            }

            else
            {
                var wishListstr = HttpContext.Request.Cookies["wishListItem"];

                List<WishListCookieItemViewModel> wishListCookieItems = null;

                wishListCookieItems = JsonConvert.DeserializeObject<List<WishListCookieItemViewModel>>(wishListstr);

                WishListCookieItemViewModel wishListCookieItem = wishListCookieItems.FirstOrDefault(x => x.PortraitId == id);

                wishListCookieItems.Remove(wishListCookieItem);

                var removeItem = JsonConvert.SerializeObject(wishListCookieItems);
                HttpContext.Response.Cookies.Append("wishListItem", removeItem);

                WishListItem item = new WishListItem();

                foreach (var items in wishListCookieItems)
                {
                    Portrait portrait = _context.Portraits.Include(x => x.PortraitImages).FirstOrDefault(x => x.Id == items.PortraitId);

                    item = new WishListItem
                    {
                        Portrait = portrait
                    };

                    wishListItems.Add(item);
                }
            }

            return PartialView("_wishListPartial", wishListItems);
        }
    }
}
