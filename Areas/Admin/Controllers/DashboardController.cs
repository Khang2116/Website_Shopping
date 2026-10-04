using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopping.Data;

namespace WebShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ProductCount = await _db.Products.CountAsync();
            ViewBag.CategoryCount = await _db.Categories.CountAsync();
            ViewBag.OrderCount = await _db.Orders.CountAsync();
            return View();
        }
    }
}