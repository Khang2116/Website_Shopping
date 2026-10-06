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
            ViewBag.PendingOrderCount = await _db.Orders.CountAsync(o => o.Status == "Chờ xử lý");
            ViewBag.RevenueCount = await _db.Orders.SumAsync(o => o.TotalAmount);

            var today = DateTime.Today;
            var sevenDaysAgo = today.AddDays(-6);

            var revenueByDay = await _db.Orders
                .Where(o => o.OrderDate >= sevenDaysAgo)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(o => o.TotalAmount) })
                .ToListAsync();

            var revenueChartData = new List<object>();
            for (int i = 0; i < 7; i++)
            {
                var day = sevenDaysAgo.AddDays(i);
                var match = revenueByDay.FirstOrDefault(r => r.Date == day);
                revenueChartData.Add(new
                {
                    period = day.ToString("dd/MM"),
                    revenue = match?.Total ?? 0
                });
            }
            ViewBag.RevenueChartData = revenueChartData;

            return View();
        }
    }
}