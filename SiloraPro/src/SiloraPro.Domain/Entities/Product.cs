namespace SiloraPro.Domain.Entities;

/// <summary>
/// كيان المنتج - يمثل بيانات المنتج في النظام
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;

    // علاقات
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
