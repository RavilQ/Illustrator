using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using Illustration.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Illustration.Controllers
{
    public class ShopController : Controller
    {

        private readonly IllustratorDbContext _context;

        public ShopController(IllustratorDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? search,int? TagId = null,int? categoriId = null, string? sort = "AtoZ", decimal? maxPrice = null, decimal? minPrice = null, int? page = 1)
        {
            var portraits = _context.Portraits.AsQueryable();
            var categories = _context.Categories.ToList();
            var tags = _context.Tags.ToList();

            ShopViewModel viewModel = new ShopViewModel { 
                Categories = categories,
                Tags = tags,
                MaxPrice = portraits.Max(x => x.SalePrice),
                MinPrice = portraits.Min(x => x.SalePrice),
            };

            if (search != null)
            {
                portraits = portraits.Where(x => x.Name.Contains(search));
            }

            if (TagId != null)
            {
                portraits = portraits.Include(x => x.PortraitTags).ThenInclude(x => x.Tag)
                    .Where(x => x.PortraitTags.Any(m => TagId == m.TagId) && x.IsSpecial);
            }

            if (categoriId != null)
            {
                portraits = portraits.Include(x => x.PortraitCategories).ThenInclude(x => x.Category)
                    .Where(x => x.PortraitCategories.Any(m => categoriId == m.CategoryId) && x.IsSpecial);
            }

            switch (sort)
            {
                case "ZtoA":
                    portraits = portraits.OrderByDescending(x => x.Name);
                    break;
                case "HightoLow":
                    portraits = portraits.OrderByDescending(x => x.SalePrice - (x.SalePrice * (x.DiscountPercent / 100)));
                    break;
                case "LowtoHigh":
                    portraits = portraits.OrderBy(x => x.SalePrice - (x.SalePrice * (x.DiscountPercent / 100)));
                    break;
                default:
                    portraits = portraits.OrderBy(x => x.Name);
                    break;
            }

            if (minPrice != null && maxPrice != null)
            {
                portraits = portraits.Where(x => x.SalePrice >= minPrice && x.SalePrice <= maxPrice);
            }

            ViewBag.SelectedMinPrice = minPrice ?? viewModel.MinPrice;
            ViewBag.SelectedMaxPrice = maxPrice ?? viewModel.MaxPrice;

            viewModel.Portraits = portraits.Include(x => x.PortraitImages)
               .Include(x => x.PortraitCategories).ThenInclude(x => x.Category)
               .Include(x => x.PortraitTags).ThenInclude(x => x.Tag).Where(x=>x.IsSpecial).ToList();

            int pageSize = 9;

            Pagination<Portrait> paginatedList = new Pagination<Portrait>();
            ViewBag.portraits = paginatedList.GetPagedNames(viewModel.Portraits, page, pageSize);

            ViewBag.pageSize = pageSize;
            ViewBag.pageNumber = page;

            if (ViewBag.portraits == null)
            {
                return View("Error");
            }

            return View(viewModel);
        }
    }
}
