namespace WebShopping.Models;

public static class DemoProducts
{
    public static List<Product> SampleList => new()
    {
        new Product { Id = 0, Name = "Áo thun basic",     Price = 250000, ImageUrl = "product-1.jpg" },
        new Product { Id = 0, Name = "Áo sơ mi trắng",    Price = 350000, ImageUrl = "product-2.jpg" },
        new Product { Id = 0, Name = "Quần jean nam",     Price = 450000, ImageUrl = "product-3.jpg" },
        new Product { Id = 0, Name = "Quần kaki",         Price = 400000, ImageUrl = "product-4.jpg" },
        new Product { Id = 0, Name = "Đầm dạ hội",        Price = 550000, ImageUrl = "product-5.jpg" },
        new Product { Id = 0, Name = "Váy suông",         Price = 320000, ImageUrl = "product-6.jpg" },
        new Product { Id = 0, Name = "Túi xách da",       Price = 600000, ImageUrl = "product-7.jpg" },
        new Product { Id = 0, Name = "Giày sneaker",      Price = 480000, ImageUrl = "product-8.jpg" },
        new Product { Id = 0, Name = "Nón lưỡi trai",     Price = 150000, ImageUrl = "product-9.jpg" },
        new Product { Id = 0, Name = "Áo khoác denim",    Price = 520000, ImageUrl = "product-10.jpg" },
        new Product { Id = 0, Name = "Chân váy jean",     Price = 380000, ImageUrl = "product-11.jpg" },
        new Product { Id = 0, Name = "Set đồ thể thao",   Price = 470000, ImageUrl = "product-12.jpg" },
    };
}