using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace Illustration.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "SuperAdmin")]
    public class SliderController : Controller
    {
        private readonly IllustratorDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(IllustratorDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int? page =1)
        {
            int pageSize = 5;

            var sliders = _context.Sliders.ToList();
            Pagination<Slider> paginatedList = new Pagination<Slider>();

            ViewBag.slider = paginatedList.GetPagedNames(sliders, page, pageSize);
            ViewBag.pageSize = pageSize;
            ViewBag.pageNumber = page;

            if (ViewBag.slider == null)
            {
                return View("Error");
            }

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (slider == null)
            {
                ModelState.AddModelError("", "Propertis cannot be empty !!");
                return View();
            }

            if (!checkImageFile(slider.ImageFile))
            {
                ModelState.AddModelError("ImageFile", "Image length has been 2mb and type of jpeg or png");
                return View();
            }


            slider.Image = FileHelper.Save(slider.ImageFile, _env.WebRootPath, "Uploads/Sliders");

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var slider = _context.Sliders.FirstOrDefault(x => x.Id == id);

            if (slider == null)
            {
                return View("Error");
            }

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(Slider newSlider)
        {
            if (newSlider == null)
            {
                ModelState.AddModelError("", "Properties cannot be empty !!");
                return View();
            }

            var slider = _context.Sliders.FirstOrDefault(x => x.Id == newSlider.Id);

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (newSlider.ImageFile != null)
            {
                if (!checkImageFile(newSlider.ImageFile))
                {
                    ModelState.AddModelError("ImageFile", "Image length has been 2mb and type of jpeg or png");
                    return View();
                }

                FileHelper.Delete(_env.WebRootPath, "Uploads/Sliders", slider.Image);
                slider.Image = FileHelper.Save(newSlider.ImageFile, _env.WebRootPath, "Uploads/Sliders");
            }

            slider.Title = newSlider.Title;
            slider.Text = newSlider.Text;
            slider.IsShow = newSlider.IsShow;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var Slider = _context.Sliders.FirstOrDefault(x => x.Id == id);

            if (Slider == null)
            {
                return View("Error");
            }

            FileHelper.Delete(_env.WebRootPath, "Uploads/Sliders", Slider.Image);

            _context.Sliders.Remove(Slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
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
