using ClosedXML.Excel;
using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Drawing;

namespace Illustration.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "SuperAdmin,Admin")]
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

            if (slider.Waitlist==0 || _context.Sliders.Any(x=>x.Waitlist==slider.Waitlist))
            {
                slider.Waitlist = _context.Sliders.Where(x => x.Id != slider.Id).Max(x => x.Waitlist) + 1;
            }


            var stream = slider.ImageFile.OpenReadStream();
            var imagee = Image.FromStream(stream);

            var resizedImage = imagee.GetThumbnailImage(1920, 1100, null, IntPtr.Zero);

            using var ms = new MemoryStream();
            resizedImage.Save(ms, imagee.RawFormat);
            ms.Position = 0;

            var resizedFile = new FormFile(ms, 0, ms.Length, slider.ImageFile.Name, slider.ImageFile.FileName);
            slider.Image = FileHelper.Save(resizedFile, _env.WebRootPath, "Uploads/Sliders");

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
                var stream = newSlider.ImageFile.OpenReadStream();
                var imagee = Image.FromStream(stream);

                var resizedImage = imagee.GetThumbnailImage(1920, 1100, null, IntPtr.Zero);

                using var ms = new MemoryStream();
                resizedImage.Save(ms, imagee.RawFormat);
                ms.Position = 0;

                var resizedFile = new FormFile(ms, 0, ms.Length, newSlider.ImageFile.Name, newSlider.ImageFile.FileName);
                slider.Image = FileHelper.Save(resizedFile, _env.WebRootPath, "Uploads/Sliders");
            }

            slider.Title = newSlider.Title;
            slider.Text = newSlider.Text;
            slider.IsShow = newSlider.IsShow;

            if (newSlider.Waitlist == 0 || _context.Sliders.Any(x => x.Waitlist == newSlider.Waitlist))
            {
                slider.Waitlist = _context.Sliders.Where(x=>x.Id!=slider.Id).Max(x => x.Waitlist) + 1;
            }
            else
            {
                slider.Waitlist = newSlider.Waitlist;
            }


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

        public IActionResult ExportAsExcell()
        {
            var category = _context.Categories.AsQueryable();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ForExel.ToDataTable<Category>(category.ToList()));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{DateTime.UtcNow.AddHours(4).Date}-houses.xlsx");
                }
            }

            return View();
        }
    }
}
