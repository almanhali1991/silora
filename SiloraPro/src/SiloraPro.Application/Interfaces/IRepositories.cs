namespace SiloraPro.Application.Interfaces;

using SiloraPro.Domain.Entities;

/// <summary>
/// واجهة مستودع العملاء
/// </summary>
public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
    Task<IEnumerable<Customer>> SearchAsync(string searchTerm);
}

/// <summary>
/// واجهة مستودع المنتجات
/// </summary>
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    Task<IEnumerable<Product>> SearchAsync(string searchTerm);
    Task<bool> IsBarcodeUniqueAsync(string barcode, int? excludeId = null);
}

/// <summary>
/// واجهة مستودع الطلبات
/// </summary>
public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<Order> AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(int id);
    Task<IEnumerable<Order>> GetByCustomerAsync(int customerId);
    Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}
