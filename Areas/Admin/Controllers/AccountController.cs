using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Website_Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
    }
}