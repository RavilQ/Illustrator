using Facebook;
using Illustration.DAL;
using Illustration.Helper;
using Illustration.Models;
using Illustration.ViewModel;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using System.Data;
using System.Security.Claims;
using Newtonsoft.Json;
using static DotNetOpenAuth.OpenId.Extensions.AttributeExchange.WellKnownAttributes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using System.Drawing;
using Microsoft.AspNetCore.Http;

namespace Illustration.Controllers
{
    public class AccountController : Controller
    {
        private readonly IllustratorDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IllustratorDbContext context, IWebHostEnvironment env, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

            var configuration = GetConfiguration();
            appId = "1198809320739698";
            appSecret = "4c262dd94602bd167d23c4cfedd13c59";
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            var admin = _context.AppUsers.FirstOrDefault(x => x.HasMember == false);
            ViewBag.admin = admin.Id;
   
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            AccountDetailViewModel viewModel = new AccountDetailViewModel
            {
                Fullname = user.Fullname,
                Username = user.UserName,
                Email = user.Email
            };

            var portraits = _context.Portraits.Include(x => x.PortraitImages).Where(x => x.AppUserId == user.Id && x.StockStatus==true).ToList();

            ProfileViewModel model = new ProfileViewModel();
            model.Portraits = portraits;
            model.WishListItem = _context.WishListItems.Include(x => x.Portrait).ThenInclude(x=>x.PortraitImages).Where(x => x.AppUserId == user.Id).ToList();
            model.MyOrders = _context.Orders.Include(x => x.Portrait).ThenInclude(x => x.PortraitImages).Where(x => x.AppUserId == user.Id).ToList();
            model.User = user;
            model.ViewModel = viewModel;
            model.SaleOrders = _context.Orders.Include(x=>x.Portrait).Include(x=>x.AppUser).Where(x =>portraits.Contains(x.Portrait)).ToList();
            model.Messages = _context.ContactMessages.Include(x => x.AppUser).Where(x=>x.AppUserId==user.Id).ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View(model);
        }


        [Authorize(Roles = "Member")]
        [HttpPost]
        public async Task<IActionResult> Profile(AccountDetailViewModel memberVm)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return View("Error");
            }

            AccountDetailViewModel viewModel = new AccountDetailViewModel
            {
                Fullname = user.Fullname,
                Username = user.UserName,
                Email = user.Email
            };

            ProfileViewModel model = new ProfileViewModel();
            model.Portraits = _context.Portraits.Include(x => x.PortraitImages).Where(x => x.AppUserId == user.Id).ToList();
            model.WishListItem = _context.WishListItems.Include(x => x.Portrait).ThenInclude(x => x.PortraitImages).Where(x => x.AppUserId == user.Id).ToList();
            model.MyOrders = _context.Orders.Include(x => x.Portrait).ThenInclude(x => x.PortraitImages).Where(x => x.AppUserId == user.Id).ToList();
            model.User = user;
            model.ViewModel = viewModel;
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (memberVm.Username == null || memberVm.Email == null)
            {
                ModelState.AddModelError("Username", "Chose another Username !!");
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View(model);
            }

            if (user.NormalizedUserName != memberVm.Username.ToUpper() && await _userManager.FindByNameAsync(memberVm.Username) != null)
            {
                ModelState.AddModelError("Username", "Chose another Username !!");
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View(model);
            }

            if (user.NormalizedEmail != memberVm.Email.ToUpper() && await _userManager.FindByEmailAsync(memberVm.Email) != null)
            {
                ModelState.AddModelError("Email", "Chose another Email !!");
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View(model);
            }

            user.UserName = memberVm.Username;
            user.Fullname = memberVm.Fullname;
            user.Email = memberVm.Email;

            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, false);

            if (memberVm.PosterImage != null)
            {
                if (!checkImageFile(memberVm.PosterImage))
                {
                    ModelState.AddModelError("PosterImage", "Image is incorrect");
                    return RedirectToAction("Profile");

                }

            }

            if (memberVm.PosterImage != null)
            {
                var newName = FileHelper.Save(memberVm.PosterImage, _env.WebRootPath, "Uploads/Users");
                if (user.Image != null)
                {
                    FileHelper.Delete(_env.WebRootPath, "Uploads/Users", user.Image);
                }
                user.Image = newName;
                _context.SaveChanges();
            }


            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View(model);
            }


            if (memberVm.NewPassword != null && memberVm.ConfirmPassword != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, memberVm.CurrentPassword);

                if (result)
                {
                    var netice = await _userManager.ChangePasswordAsync(user, memberVm.CurrentPassword, memberVm.ConfirmPassword);

                    if (netice == null)
                    {
                        ViewBag.Categories = _context.Categories.ToList();
                        ViewBag.Tags = _context.Tags.ToList();
                        return View(model);
                    }
                }
            }

            return RedirectToAction("Profile");
        }


        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Portrait portrait)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View();
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return View("Error");
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

            if (portrait.OtherImages != null)
            {
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
            }

            var stream = portrait.PosterImage.OpenReadStream();
            var imagee = Image.FromStream(stream);

            var resizedImage = imagee.GetThumbnailImage(594, 787, null, IntPtr.Zero);

            using var ms = new MemoryStream();
            resizedImage.Save(ms, imagee.RawFormat);
            ms.Position = 0;

            var resizedFile = new FormFile(ms, 0,ms.Length, portrait.PosterImage.Name, portrait.PosterImage.FileName);

            var newName = FileHelper.Save(resizedFile, _env.WebRootPath, "Uploads/Portraits");

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
                    var stream2 = images.OpenReadStream();
                    var imagee2 = Image.FromStream(stream2);

                    var resizedImage2 = imagee2.GetThumbnailImage(768, 1013, null, IntPtr.Zero);

                    using var ms2 = new MemoryStream();
                    resizedImage2.Save(ms2, imagee2.RawFormat);
                    ms2.Position = 0;

                    var resizedFile2 = new FormFile(ms2, 0, ms2.Length, images.Name, images.FileName);

                    PortraitImage imagess = new PortraitImage
                    {

                        Image = FileHelper.Save(resizedFile2, _env.WebRootPath, "Uploads/Portraits"),
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

            portrait.AppUserId = user.Id;

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
                var stream = portrait.PosterImage.OpenReadStream();
                var imagee = Image.FromStream(stream);

                var resizedImage = imagee.GetThumbnailImage(594, 787, null, IntPtr.Zero);

                using var ms = new MemoryStream();
                resizedImage.Save(ms, imagee.RawFormat);
                ms.Position = 0;

                var resizedFile = new FormFile(ms, 0, ms.Length, portrait.PosterImage.Name, portrait.PosterImage.FileName);

                var newName = FileHelper.Save(resizedFile, _env.WebRootPath, "Uploads/Portraits");
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
                    var stream2 = images.OpenReadStream();
                    var imagee2 = Image.FromStream(stream2);

                    var resizedImage2 = imagee2.GetThumbnailImage(768, 1013, null, IntPtr.Zero);

                    using var ms2 = new MemoryStream();
                    resizedImage2.Save(ms2, imagee2.RawFormat);
                    ms2.Position = 0;

                    var resizedFile2 = new FormFile(ms2, 0, ms2.Length, images.Name, images.FileName);

                    PortraitImage imagess = new PortraitImage
                    {

                        Image = FileHelper.Save(resizedFile2, _env.WebRootPath, "Uploads/Portraits"),
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


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel memberVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(memberVm.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email is wrong !!");
                return View();
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains("Member"))
            {
                ModelState.AddModelError("", "Email or password is wrong !!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, memberVm.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Password", "Password is wrong !!");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterViewModel registerVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await _userManager.FindByEmailAsync(registerVm.Email) != null)
            {
                ModelState.AddModelError("Email", "That email already taken!!");
                return View();
            }

            if (await _userManager.FindByNameAsync(registerVm.Username) != null)
            {
                ModelState.AddModelError("Username", "That Username alreaady taken!!");
            }

            string newName = null;

            if (registerVm.PosterImage == null)
            {

            }
            else
            {
                if (!checkImageFile(registerVm.PosterImage))
                {
                    ModelState.AddModelError("PosterImage", "Image is incorrect");
                    return View();

                }

              newName = FileHelper.Save(registerVm.PosterImage, _env.WebRootPath, "Uploads/Users");
            }

            AppUser user = new AppUser
            {

                Fullname = registerVm.Fullname,
                UserName = registerVm.Username,
                Email = registerVm.Email,
                Image = newName,
                HasMember = true

            };

            var result = await _userManager.CreateAsync(user, registerVm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }

            await _userManager.AddToRoleAsync(user, "Member");

            await _signInManager.PasswordSignInAsync(user, registerVm.Password, false, false);

            return RedirectToAction("Index", "Home");
        }

        //public async Task<IActionResult> test()
        //{
        //    IdentityRole role1 = new IdentityRole("Member");
        //    IdentityRole role2 = new IdentityRole("SuperAdmin");
        //    IdentityRole role3 = new IdentityRole("Admin");

        //    await _roleManager.CreateAsync(role1);
        //    await _roleManager.CreateAsync(role2);
        //    await _roleManager.CreateAsync(role3);

        //    return Ok();
        //}

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }


        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
            {
                AppUser user = new AppUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };

                IdentityResult identResult = await _userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Member");
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                return NotFound();
            }
        }


        string appId = string.Empty;
        string appSecret = string.Empty;

        public IConfiguration GetConfiguration()
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Headers["Referer"].ToString());
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public IActionResult Facebook()
        {
            var fb = new FacebookClient();

            var loginurl = fb.GetLoginUrl(new
            {
                client_id = appId,
                client_secret = appSecret,
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginurl.AbsoluteUri);
        }

        public async Task<IActionResult> FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = appId,
                client_secret = appSecret,
                redirect_uri = "https://localhost:7237/Account/FacebookCallback",
                code = code
            });
            var accesstoken = result.access_token;
            fb.AccessToken = accesstoken;
            dynamic data = fb.Get("me?fileds=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");
            string helpme = data.name;
            AppUser user = new AppUser
            {
                UserName = helpme.Replace(" ", "")
            };

            var oldUser = await _userManager.FindByNameAsync(user.UserName);

            if (oldUser != null)
            {
                await _signInManager.SignInAsync(oldUser, false);
                return RedirectToAction("Index", "Home");
            }

            IdentityResult identResult = await _userManager.CreateAsync(user);
            if (identResult.Succeeded)
            {
              await _userManager.AddToRoleAsync(user, "Member");
              await _signInManager.SignInAsync(user, false);
               return RedirectToAction("Index", "Home");             
            }
            else
            {
                return View("Error");
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            AppUser user = await _userManager.FindByEmailAsync(forgotPassword.Email);

            if (user == null)
            {
                return View("Error");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action("VerifyPasswordReset", "account", new { email = user.Email, token = token }, Request.Scheme);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("iillustrator@yandex.ru"));
            email.To.Add(MailboxAddress.Parse($"{user.Email}"));
            email.Subject = "Reset Your Password";
            email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>Hi.{user.Fullname} click <a href=\"{link}\">link</a> for the change your password</h1>" };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.yandex.com", 465, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate("iillustrator@yandex.ru", "illustrator123$");
            smtp.Send(email);
            smtp.Disconnect(true);

            //using var smtp = new SmtpClient();
            //smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            //smtp.Authenticate("della82@ethereal.email", "eQRmUMdy7eHWZwmArv");
            //smtp.Send(email);
            //smtp.Disconnect(true);

            return View();
        }

        public async Task<IActionResult> VerifyPasswordReset(string email, string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return View("Error");
            }

            if (!await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
            {
                return View("Error");
            }

            TempData["email"] = email;
            TempData["token"] = token;
            return RedirectToAction("ResetPassword");
        }

        public IActionResult ResetPassword()
        {
            var email = TempData["email"];
            var token = TempData["token"];

            ViewBag.email = email;
            ViewBag.token = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel resetVm)
        {
            AppUser user = await _userManager.FindByEmailAsync(resetVm.Email);

            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetVm.Token, resetVm.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("Password", err.Description);
                    return View();
                }
            }

            return RedirectToAction("login");

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
                var wishListItem = _context.WishListItems.FirstOrDefault(x => x.PortraitId == id && x.AppUserId == user.Id);
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

            return RedirectToAction("Profile","Account");
        }

        public IActionResult Order(int id)
        {
            Order order = new Order {

                Portrait = _context.Portraits.FirstOrDefault(x => x.Id == id)

            };
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Order(Order order,int id)
        {
            if (id != 0)
            {
                order.PortraitId = id;
            }
            else
            {
                return View("Error");
            }
           

            if (!ModelState.IsValid)
            {
                order.Portrait = _context.Portraits.FirstOrDefault(x => x.Id == id);
                return View(order);
            }

            order.Status = Enum.OrderStatus.Pending;

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            MyOrder myOrder = new MyOrder
            {
                PortraitId = id,
                Status = Enum.OrderStatus.Pending
            };

            if (user != null)
            {
                order.AppUserId = user.Id;
                myOrder.AppUserId = user.Id;
            }

            var wishListItem = _context.WishListItems.FirstOrDefault(x => x.PortraitId == id);
            if (wishListItem != null)
            {
                _context.WishListItems.Remove(wishListItem);
            }

            order.Id = 0;
            _context.Orders.Add(order);
            _context.MyOrders.Add(myOrder);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult SaleOrderEdit(int id)
        {
            var order = _context.Orders.Include(x => x.AppUser).Include(x=>x.Portrait).ThenInclude(x=>x.PortraitImages).FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return View("Error");
            }

            return View(order);
        }

        public IActionResult Approve(int id)
        {
            var order = _context.Orders.FirstOrDefault(x => x.Id == id);
            order.Status = Enum.OrderStatus.Accepted;
            var portrait = _context.Portraits.FirstOrDefault(x => x.Id == order.PortraitId);
            portrait.StockStatus = false;
            _context.SaveChanges();

            return RedirectToAction("Profile");
        }

        public IActionResult Reject(int id)
        {
            var order = _context.Orders.FirstOrDefault(x => x.Id == id);

            order.Status = Enum.OrderStatus.Rejected;
            _context.SaveChanges();

            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> InboxMessageSend(string MyMessage)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (MyMessage.Length>800)
            {
                return View();
            }

            ContactMessage message = new ContactMessage { 
            
                AppUserId = user.Id,
                IsMember = true,
                Text = MyMessage

            };

            _context.ContactMessages.Add(message);
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
