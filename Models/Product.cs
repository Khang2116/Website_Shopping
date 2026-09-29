using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopping.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
    [StringLength(200)]
    [Display(Name = "Tên sản phẩm")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
    [Column(TypeName = "decimal(18,0)")]
    [Display(Name = "Giá bán")]
    public decimal Price { get; set; }

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [StringLength(255)]
    [Display(Name = "Hình ảnh")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn danh mục")]
    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}