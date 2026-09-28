using Microsoft.AspNetCore.Mvc;

namespace WebShopping.Controllers;

public class ProductController : Controller
{
    public IActionResult Shop() => View();
    public IActionResult ProductDetails(int id) => View();
    public IActionResult Cart() => View();
    public IActionResult Checkout() => View();
}