namespace SiloraPro.Domain.Entities;

/// <summary>
/// كيان الطلب - يمثل بيانات الطلب في النظام
/// </summary>
public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "جديد"; // جديد، قيد المعالجة، مكتمل، ملغى
    public string Notes { get; set; } = string.Empty;

    // علاقات
    public Customer? Customer { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
