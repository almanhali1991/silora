using SiloraPro.Application.Interfaces;
using SiloraPro.Domain.Entities;
using SiloraPro.Infrastructure.Repositories;

namespace SiloraPro.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<Customer>> GetAllAsync() => await _customerRepository.GetAllAsync();
    public async Task<Customer?> GetByIdAsync(int id) => await _customerRepository.GetByIdAsync(id);
    public async Task<Customer> CreateAsync(Customer customer) => await _customerRepository.CreateAsync(customer);
    public async Task<Customer> UpdateAsync(Customer customer) => await _customerRepository.UpdateAsync(customer);
    public async Task DeleteAsync(int id) => await _customerRepository.DeleteAsync(id);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetAllAsync() => await _productRepository.GetAllAsync();
    public async Task<Product?> GetByIdAsync(int id) => await _productRepository.GetByIdAsync(id);
    public async Task<Product> CreateAsync(Product product) => await _productRepository.CreateAsync(product);
    public async Task<Product> UpdateAsync(Product product) => await _productRepository.UpdateAsync(product);
    public async Task DeleteAsync(int id) => await _productRepository.DeleteAsync(id);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<Order>> GetAllAsync() => await _orderRepository.GetAllAsync();
    public async Task<Order?> GetByIdAsync(int id) => await _orderRepository.GetByIdAsync(id);
    public async Task<Order> CreateAsync(Order order) => await _orderRepository.CreateAsync(order);
    public async Task DeleteAsync(int id) => await _orderRepository.DeleteAsync(id);
}
