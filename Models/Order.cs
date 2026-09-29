using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopping.Models;

public class Order
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    [StringLength(100)]
    [Display(Name = "Họ tên khách hàng")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
    [StringLength(15)]
    [Display(Name = "Số điện thoại")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
    [StringLength(255)]
    [Display(Name = "Địa chỉ giao hàng")]
    public string Address { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Ghi chú")]
    public string? Note { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(18,0)")]
    [Display(Name = "Tổng tiền")]
    public decimal TotalAmount { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = "Chờ xử lý";

    public ICollection<OrderDetail>? OrderDetails { get; set; }
}