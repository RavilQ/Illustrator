using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using Illustration.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Illustration.Controllers
{
    public class AccountController : Controller
    {
        private readonly IllustratorDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AccountController(IllustratorDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Profile()
        {
            ProfileViewModel model = new ProfileViewModel();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(Portrait portrait)
        {
            portrait.AppUser = null;
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return RedirectToAction("Profile");
            }

            foreach (var item in portrait.CategoryIds)
            {
                if (!_context.Categories.Any(x => x.Id == item))
                {
                    return View("Error");
                }

            }

            foreach (var item in portrait.TagIds)
            {
                if (!_context.Tags.Any(x => x.Id == item))
                {
                    return View("Error");
                }

            }


            if (!checkImageFile(portrait.PosterImage))
            {
                ModelState.AddModelError("PosterImage", "Image is incorrect");
                return RedirectToAction("Profile");

            }

            foreach (var item in portrait.OtherImages)
            {
                if (!checkImageFile(item))
                {
                    ModelState.AddModelError("OtherImages", "Image is incorrect");
                    return RedirectToAction("Profile");

                }
            }

            var newName = FileHelper.Save(portrait.PosterImage, _env.WebRootPath, "Uploads/Portraits");

            PortraitImage image = new PortraitImage
            {

                Image = newName,
                ImageStatus = true,
                Portrait = portrait

            };

            portrait.PortraitImages.Add(image);

            if (portrait.OtherImages != null)
            {
                foreach (var images in portrait.OtherImages)
                {
                    PortraitImage imagess = new PortraitImage
                    {

                        Image = FileHelper.Save(images, _env.WebRootPath, "Uploads/Portraits"),
                        ImageStatus = false,
                        Portrait = portrait
                    };

                    portrait.PortraitImages.Add(imagess);
                }
            }

            foreach (var item in portrait.CategoryIds)
            {
                PortraitCategory category = new PortraitCategory
                {


                    CategoryId = item,
                    PortraitId = portrait.Id

                };

                portrait.PortraitCategories.Add(category);
            }
            _context.Portraits.Add(portrait);
            _context.SaveChanges();

            return RedirectToAction("Profile");

        }

        public bool checkImageFile(IFormFile image)
        {

            if (image.Length > 4194304)
            {
                return false;
            }

            if (image.ContentType != "image/jpeg" && image.ContentType != "image/png")
            {
                return false;
            }

            return true;
        }
    }
}
