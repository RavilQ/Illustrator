﻿using Microsoft.AspNetCore.Mvc;

namespace Illustration.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
