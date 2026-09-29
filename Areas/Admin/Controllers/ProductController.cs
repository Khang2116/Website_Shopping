using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShopping.Data;
using WebShopping.Models;

namespace WebShopping.Areas.Admin.Controllers;

[Area("Admin")]
[AllowAnonymous]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ProductController(ApplicationDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    private async Task FillCategoryListAsync(int? selectedId = null)
    {
        var categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
        ViewBag.CategoryList = new SelectList(categories, "Id", "Name", selectedId);
    }

    public async Task<IActionResult> Index()
    {
        var products = await _db.Products.Include(p => p.Category).ToListAsync();
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        await FillCategoryListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
    {
        if (!ModelState.IsValid)
        {
            await FillCategoryListAsync(product.CategoryId);
            return View(product);
        }

        if (imageFile != null && imageFile.Length > 0)
        {
            product.ImageUrl = await SaveImageAsync(imageFile);
        }

        product.CreatedAt = DateTime.Now;
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Thêm sản phẩm thành công";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();
        await FillCategoryListAsync(product.CategoryId);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile)
    {
        if (id != product.Id) return NotFound();
        if (!ModelState.IsValid)
        {
            await FillCategoryListAsync(product.CategoryId);
            return View(product);
        }

        if (imageFile != null && imageFile.Length > 0)
        {
            product.ImageUrl = await SaveImageAsync(imageFile);
        }
        else
        {
            var existing = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            product.ImageUrl = existing?.ImageUrl;
        }

        _db.Products.Update(product);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Cập nhật sản phẩm thành công";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Xóa sản phẩm thành công";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> SaveImageAsync(IFormFile imageFile)
    {
        string uploadsFolder = Path.Combine(_env.WebRootPath, "img", "products");
        Directory.CreateDirectory(uploadsFolder);

        string fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageFile.FileName);
        string filePath = Path.Combine(uploadsFolder, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);

        return fileName;
    }
}