namespace SiloraPro.Domain.Entities;

/// <summary>
/// كيان عنصر الطلب - يمثل منتجات داخل طلب معين
/// </summary>
public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    // علاقات
    public Order? Order { get; set; }
    public Product? Product { get; set; }
}
