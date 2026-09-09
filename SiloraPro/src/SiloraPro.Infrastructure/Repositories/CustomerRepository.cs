namespace SiloraPro.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using SiloraPro.Domain.Entities;
using SiloraPro.Application.Interfaces;
using SiloraPro.Infrastructure.Data;

/// <summary>
/// مستودع العملاء - تنفيذ الوصول للبيانات
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly SiloraProDbContext _context;

    public CustomerRepository(SiloraProDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task UpdateAsync(Customer customer)
    {
        var existing = await _context.Customers.FindAsync(customer.Id);
        if (existing != null)
        {
            existing.Name = customer.Name;
            existing.Phone = customer.Phone;
            existing.Email = customer.Email;
            existing.Address = customer.Address;
            existing.IsActive = customer.IsActive;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            customer.IsActive = false; // حذف منطقي
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm)
    {
        return await _context.Customers
            .Where(c => c.IsActive && 
                       (c.Name.Contains(searchTerm) || 
                        c.Phone.Contains(searchTerm) || 
                        c.Email.Contains(searchTerm)))
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}
