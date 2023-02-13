using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using Illustration.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            model.Portraits = _context.Portraits.Include(x=>x.PortraitImages).ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Portrait portrait)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View();
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
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                ModelState.AddModelError("PosterImage", "Image is incorrect");
                return View();

            }

            foreach (var item in portrait.OtherImages)
            {
                if (!checkImageFile(item))
                {
                    ViewBag.Categories = _context.Categories.ToList();
                    ViewBag.Tags = _context.Tags.ToList();
                    ModelState.AddModelError("OtherImages", "Image is incorrect");
                    return View();

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

        public IActionResult Edit(int id)
        {
            var portrait = _context.Portraits.FirstOrDefault(x => x.Id == id);

            if (portrait == null)
            {
                return View("Error");
            }
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            return View(portrait);
        }

        [HttpPost]
        public IActionResult Edit(Portrait portrait)
        {
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

            if (portrait.PosterImage != null)
            {
                if (!checkImageFile(portrait.PosterImage))
                {
                    ModelState.AddModelError("PosterImage", "Image is incorrect");
                    return RedirectToAction("Profile");

                }

            }

            if (portrait.OtherImages != null)
            {
                foreach (var item in portrait.OtherImages)
                {
                    if (!checkImageFile(item))
                    {
                        ModelState.AddModelError("OtherImages", "Image is incorrect");
                        return RedirectToAction("Profile");

                    }
                }
            }

            

            var oldPortrait = _context.Portraits.Include(x => x.PortraitTags)
                .Include(x => x.PortraitCategories)
                .Include(x=>x.PortraitImages).FirstOrDefault(x => x.Id == portrait.Id);

            var oldPosterImage = oldPortrait.PortraitImages.FirstOrDefault(x => x.ImageStatus == true);

            if (portrait.PosterImage != null)
            {
                var newName = FileHelper.Save(portrait.PosterImage, _env.WebRootPath, "Uploads/Portraits");
                FileHelper.Delete(_env.WebRootPath, "Uploads/Portraits", oldPosterImage.Image);
                _context.PortraitImages.Remove(oldPosterImage);
                PortraitImage image = new PortraitImage
                {

                    Image = newName,
                    ImageStatus = true,
                    PortraitId = oldPortrait.Id
                };

                oldPortrait.PortraitImages.Add(image);
            }

           

            if (portrait.OtherImages != null)
            {
                var oldOtherImages = oldPortrait.PortraitImages.FindAll(x => x.ImageStatus == false);

                foreach (var item in oldOtherImages)
                {
                    FileHelper.Delete(_env.ContentRootPath, "Uploads/Portraits", item.Image);
                }

                _context.PortraitImages.RemoveRange(oldOtherImages);

                foreach (var images in portrait.OtherImages)
                {
                    PortraitImage imagess = new PortraitImage
                    {

                        Image = FileHelper.Save(images, _env.WebRootPath, "Uploads/Portraits"),
                        ImageStatus = false,
                        Portrait = portrait
                    };

                    oldPortrait.PortraitImages.Add(imagess);
                }
            }

            oldPortrait.Name = portrait.Name;
            oldPortrait.Dimention = portrait.Dimention;
            oldPortrait.Weight = portrait.Weight;
            oldPortrait.Info = portrait.Info;
            oldPortrait.IsAuktion = portrait.IsAuktion;
            oldPortrait.IsSpecial = portrait.IsSpecial;
            oldPortrait.StockStatus = portrait.StockStatus;
            oldPortrait.CreatAt = portrait.CreatAt;
            oldPortrait.CostPrice = portrait.CostPrice;
            oldPortrait.DiscountPercent = portrait.DiscountPercent;
            oldPortrait.SalePrice = portrait.SalePrice;
            oldPortrait.Desc = portrait.Desc;

            if (portrait.TagIds.Count!=0)
            {
                foreach (var item in oldPortrait.PortraitTags)
                {
                    _context.PortraitTags.Remove(item);
                }


                foreach (var item in portrait.TagIds)
                {
                    PortraitTag tag = new PortraitTag
                    {


                        TagId = item,
                        PortraitId = portrait.Id

                    };

                    oldPortrait.PortraitTags.Add(tag);
                }
            }

            if (portrait.CategoryIds.Count!=0)
            {

                foreach (var item in oldPortrait.PortraitCategories)
                {
                    _context.PortraitCategories.Remove(item);
                }


                foreach (var item in portrait.CategoryIds)
                {
                    PortraitCategory category = new PortraitCategory
                    {
                        CategoryId = item,
                        PortraitId = portrait.Id
                    };

                    oldPortrait.PortraitCategories.Add(category);
                }

            }

            _context.SaveChanges();

            return RedirectToAction("Profile");
        }

        public IActionResult Delete(int id)
        {
            var portrait = _context.Portraits.Include(x => x.PortraitImages).FirstOrDefault(x => x.Id == id);

            if (portrait == null)
            {
                return View("Error");
            }

            var posterImage = portrait.PortraitImages.FirstOrDefault(x => x.ImageStatus == true).Image;
            var otherImages = portrait.PortraitImages.FindAll(x => x.ImageStatus == false);

            FileHelper.Delete(_env.WebRootPath, "Uploads/Portraits", posterImage);

            foreach (var item in otherImages)
            {
                FileHelper.Delete(_env.WebRootPath, "Uploads/Portraits", item.Image);
            }

            _context.Portraits.Remove(portrait);
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
