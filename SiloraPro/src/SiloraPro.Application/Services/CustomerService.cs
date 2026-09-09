namespace SiloraPro.Application.Services;

using SiloraPro.Domain.Entities;
using SiloraPro.Application.Interfaces;

/// <summary>
/// خدمة إدارة العملاء
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<Customer> CreateCustomerAsync(string name, string phone, string email, string address)
    {
        var customer = new Customer
        {
            Name = name,
            Phone = phone,
            Email = email,
            Address = address,
            CreatedAt = DateTime.Now,
            IsActive = true
        };

        return await _customerRepository.AddAsync(customer);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        if (customer == null)
            throw new ArgumentNullException(nameof(customer));

        await _customerRepository.UpdateAsync(customer);
    }

    public async Task DeleteCustomerAsync(int id)
    {
        await _customerRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
    {
        return await _customerRepository.SearchAsync(searchTerm);
    }
}
