namespace WebShopping.Models;

public class CartItem
{
    public const string SessionKey = "Cart";
    
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }

    public decimal Total => Price * Quantity;
}