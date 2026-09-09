namespace SiloraPro.Application.Interfaces;

using SiloraPro.Domain.Entities;

/// <summary>
/// واجهة خدمة العملاء
/// </summary>
public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task<Customer> CreateCustomerAsync(string name, string phone, string email, string address);
    Task UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int id);
    Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm);
}

/// <summary>
/// واجهة خدمة المنتجات
/// </summary>
public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(string name, string description, decimal price, int stockQuantity, string category, string barcode);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
}

/// <summary>
/// واجهة خدمة الطلبات
/// </summary>
public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync(int customerId, List<OrderItemDto> items, string notes = "");
    Task UpdateOrderStatusAsync(int orderId, string status);
    Task DeleteOrderAsync(int orderId);
    Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
}

/// <summary>
/// نموذج نقل البيانات لعنصر الطلب
/// </summary>
public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
