using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Illustration.Area.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "SuperAdmin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
