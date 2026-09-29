using System.ComponentModel.DataAnnotations;

namespace WebShopping.Models;

public class AdminUser
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string PasswordHash { get; set; } = string.Empty;
}