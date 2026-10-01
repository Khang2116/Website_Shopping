using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopping.Data;
using WebShopping.Models;

namespace WebShopping.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;

    public ProductController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Shop(int? categoryId)
    {
        var query = _db.Products.Include(p => p.Category).AsQueryable();

        bool isFiltering = categoryId.HasValue && categoryId > 0;
        if (isFiltering)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }

        var realProducts = await query.ToListAsync();

        var displayList = new List<Product>(realProducts);

        if (!isFiltering && realProducts.Count == 0)
        {
            displayList.AddRange(DemoProducts.SampleList);
        }

        ViewBag.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
        ViewBag.SelectedCategoryId = categoryId;

        return View(displayList);
    }

    public async Task<IActionResult> ProductDetails(int id)
    {
        var product = await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();

        ViewBag.RelatedProducts = await _db.Products
            .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
            .Take(8)
            .ToListAsync();

        return View(product);
    }

    public IActionResult Cart() => View();
    public IActionResult Checkout() => View();
}