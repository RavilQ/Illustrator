using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using System.Data;
using System.Drawing;
using System.Reflection.Metadata;
using MailKit.Net.Smtp;

namespace Illustration.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class BlogController : Controller
    {
        private readonly IllustratorDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(IllustratorDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var blog = _context.Blog.First();

            return View(blog);
        }

        public IActionResult Edit()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Edit(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var oldblog = _context.Blog.First();

            if (blog.FirstImageFile != null)
            {
                if (!checkImageFile(blog.FirstImageFile))
                {
                    ModelState.AddModelError("ImageFile", "Image length has been 2mb and type of jpeg or png");
                    return View();
                }

                FileHelper.Delete(_env.WebRootPath, "Uploads/Blog", oldblog.FirstImage);
                var stream = blog.FirstImageFile.OpenReadStream();
                var imagee = Image.FromStream(stream);

                var resizedImage = imagee.GetThumbnailImage(1300, 535, null, IntPtr.Zero);

                using var ms = new MemoryStream();
                resizedImage.Save(ms, imagee.RawFormat);
                ms.Position = 0;

                var resizedFile = new FormFile(ms, 0, ms.Length, blog.FirstImageFile.Name, blog.FirstImageFile.FileName);

                oldblog.FirstImage = FileHelper.Save(resizedFile, _env.WebRootPath, "Uploads/Blog");
            }

            if (blog.SecondImageFile != null)
            {
                if (!checkImageFile(blog.SecondImageFile))
                {
                    ModelState.AddModelError("ImageFile", "Image length has been 2mb and type of jpeg or png");
                    return View();
                }

                FileHelper.Delete(_env.WebRootPath, "Uploads/Blog", oldblog.SecondImage);
                var stream = blog.SecondImageFile.OpenReadStream();
                var imagee = Image.FromStream(stream);

                var resizedImage = imagee.GetThumbnailImage(1300, 535, null, IntPtr.Zero);

                using var ms = new MemoryStream();
                resizedImage.Save(ms, imagee.RawFormat);
                ms.Position = 0;

                var resizedFile = new FormFile(ms, 0, ms.Length, blog.SecondImageFile.Name, blog.SecondImageFile.FileName);

                oldblog.SecondImage = FileHelper.Save(resizedFile, _env.WebRootPath, "Uploads/Blog");
            }

            if (blog.ThirdImageFile != null)
            {
                if (!checkImageFile(blog.ThirdImageFile))
                {
                    ModelState.AddModelError("ImageFile", "Image length has been 2mb and type of jpeg or png");
                    return View();
                }

                FileHelper.Delete(_env.WebRootPath, "Uploads/Blog", oldblog.ThirdImage);
                var stream = blog.ThirdImageFile.OpenReadStream();
                var imagee = Image.FromStream(stream);

                var resizedImage = imagee.GetThumbnailImage(1300, 535, null, IntPtr.Zero);

                using var ms = new MemoryStream();
                resizedImage.Save(ms, imagee.RawFormat);
                ms.Position = 0;

                var resizedFile = new FormFile(ms, 0, ms.Length, blog.ThirdImageFile.Name, blog.ThirdImageFile.FileName);

                oldblog.ThirdImage = FileHelper.Save(resizedFile, _env.WebRootPath, "Uploads/Blog");

            }

            if (blog.FirstTitle!=null)
            {
                oldblog.FirstTitle = blog.FirstTitle;
            }
            if (blog.FirstText!=null)
            {
                oldblog.FirstText = blog.FirstText;
            }
            if (blog.SecondTitle!=null)
            {
                oldblog.SecondTitle = blog.SecondTitle;
            }
            if (blog.SecondText!=null)
            {
                oldblog.SecondText = blog.SecondText;
            }
            if (blog.ThirdTitle!=null)
            {
                oldblog.ThirdTitle = blog.ThirdTitle;
            }
            if (blog.ThirdText!=null)
            {
                oldblog.ThirdText = blog.ThirdText;
            }
            if (blog.FirstGreyText!=null)
            {
                oldblog.FirstGreyText = blog.FirstGreyText;
            }
            if (blog.FirstGreyAuthor!= null)
            {
                oldblog.FirstGreyAuthor = blog.FirstGreyAuthor;
            }
            if (blog.SecondGreyText!=null)
            {
                oldblog.SecondGreyText = blog.SecondGreyText;
            }           

            _context.SaveChanges();

            var subscriber = _context.Subscribers.Include(x=>x.AppUser).First();

            var user = _context.AppUsers.FirstOrDefault(x => x.Id == subscriber.AppUser.Id);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("iillustrator@yandex.ru"));
            email.To.Add(MailboxAddress.Parse($"{user.Email}"));
            email.Subject = "Congratulations!!";
            email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>Oh, look what new here!!</h1>" };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.yandex.com", 465, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate("iillustrator@yandex.ru", "illustrator123$");
            smtp.Send(email);
            smtp.Disconnect(true);

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
