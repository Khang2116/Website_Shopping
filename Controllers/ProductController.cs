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

        var products = await query.ToListAsync();

        ViewBag.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
        ViewBag.SelectedCategoryId = categoryId;

        return View(products);
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

    private List<CartItem> GetCart()
    {
        return HttpContext.Session.GetObject<List<CartItem>>(CartItem.SessionKey) ?? new List<CartItem>();
    }

    private void SaveCart(List<CartItem> cart)
    {
        HttpContext.Session.SetObject(CartItem.SessionKey, cart);
    }

    public IActionResult Cart()
    {
        return View(GetCart());
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1, string? returnUrl = null)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product == null) return NotFound();

        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId);

        if (item != null)
        {
            item.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = quantity
            });
        }

        SaveCart(cart);
        TempData["Success"] = "Đã thêm vào giỏ hàng";

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return RedirectToAction(nameof(Cart));
    }

    [HttpPost]
    public IActionResult UpdateQuantity(int productId, int quantity)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId);

        if (item != null)
        {
            if (quantity <= 0) cart.Remove(item);
            else item.Quantity = quantity;
        }

        SaveCart(cart);
        return RedirectToAction(nameof(Cart));
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        var cart = GetCart();
        cart.RemoveAll(c => c.ProductId == productId);
        SaveCart(cart);
        return RedirectToAction(nameof(Cart));
    }

    [HttpPost]
    public IActionResult ClearCart()
    {
        SaveCart(new List<CartItem>());
        return RedirectToAction(nameof(Cart));
    }

    public IActionResult Checkout()
    {
        var cart = GetCart();
        if (!cart.Any())
        {
            TempData["Error"] = "Giỏ hàng đang trống";
            return RedirectToAction(nameof(Cart));
        }

        ViewBag.Cart = cart;
        return View(new Order());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(Order order)
    {
        var cart = GetCart();
        if (!cart.Any())
        {
            TempData["Error"] = "Giỏ hàng đang trống";
            return RedirectToAction(nameof(Cart));
        }

        ModelState.Remove(nameof(Order.OrderDate));
        ModelState.Remove(nameof(Order.Status));
        ModelState.Remove(nameof(Order.TotalAmount));

        if (!ModelState.IsValid)
        {
            ViewBag.Cart = cart;
            return View(order);
        }

        order.OrderDate = DateTime.Now;
        order.Status = "Chờ xử lý";
        order.TotalAmount = cart.Sum(c => c.Total);
        order.OrderDetails = cart.Select(c => new OrderDetail
        {
            ProductId = c.ProductId,
            Quantity = c.Quantity,
            UnitPrice = c.Price
        }).ToList();

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        SaveCart(new List<CartItem>());

        return RedirectToAction(nameof(OrderSuccess), new { id = order.Id });
    }

    public async Task<IActionResult> OrderSuccess(int id)
    {
        var order = await _db.Orders
            .Include(o => o.OrderDetails).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();
        return View(order);
    }
}