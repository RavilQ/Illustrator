using Microsoft.AspNetCore.Mvc;

namespace Illustration.Controllers
{
    public class PortraitController : Controller
    {
        public IActionResult Detail()
        {
            return View();
        }
    }
}
