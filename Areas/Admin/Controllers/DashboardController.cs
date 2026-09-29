using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize(AuthenticationSchemes = "AdminCookie")]
    [AllowAnonymous]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}