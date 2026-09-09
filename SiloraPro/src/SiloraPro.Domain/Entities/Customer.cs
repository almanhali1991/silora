namespace SiloraPro.Domain.Entities;

/// <summary>
/// كيان العميل - يمثل بيانات العميل في النظام
/// </summary>
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;

    // علاقات
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
