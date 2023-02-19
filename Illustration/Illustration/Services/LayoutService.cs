using Illustration.DAL;
using Illustration.Models;
using Illustration.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Security.Claims;


namespace Illustration.Services
{
    public class LayoutService
    {
        private readonly IllustratorDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(IllustratorDbContext context, IHttpContextAccessor accessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _accessor = accessor;
            _userManager = userManager;

        }

        public List<WishListItem> GetWishListItems()
        {
            List<WishListItem> wishList = new List<WishListItem>();

            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                string UserId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                wishList = _context.WishListItems.Include(x => x.Portrait).ThenInclude(x => x.PortraitImages).ToList();
            }
            else
            {
                var wishListStr = _accessor.HttpContext.Request.Cookies["wishListITem"];

                List<WishListCookieItemViewModel> cookieItems = new List<WishListCookieItemViewModel>();

                if (wishListStr != null)
                {
                    cookieItems = JsonConvert.DeserializeObject<List<WishListCookieItemViewModel>>(wishListStr);
                }
                WishListItem item = new WishListItem();

                foreach (var items in cookieItems)
                {
                    Portrait portrait = _context.Portraits.Include(x => x.PortraitImages).FirstOrDefault(x => x.Id == items.PortraitId);

                    item = new WishListItem
                    {
                        Portrait = portrait
                    };

                    wishList.Add(item);
                }
            }

            return wishList;
        }
    }
}
