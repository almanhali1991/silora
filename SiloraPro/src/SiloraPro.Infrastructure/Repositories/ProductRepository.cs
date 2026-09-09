namespace SiloraPro.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using SiloraPro.Domain.Entities;
using SiloraPro.Application.Interfaces;
using SiloraPro.Infrastructure.Data;

/// <summary>
/// مستودع المنتجات - تنفيذ الوصول للبيانات
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly SiloraProDbContext _context;

    public ProductRepository(SiloraProDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
    }

    public async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        var existing = await _context.Products.FindAsync(product.Id);
        if (existing != null)
        {
            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.StockQuantity = product.StockQuantity;
            existing.Category = product.Category;
            existing.Barcode = product.Barcode;
            existing.IsActive = product.IsActive;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            product.IsActive = false; // حذف منطقي
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
    {
        return await _context.Products
            .Where(p => p.IsActive && 
                       (p.Name.Contains(searchTerm) || 
                        p.Description.Contains(searchTerm) ||
                        p.Category.Contains(searchTerm) ||
                        p.Barcode.Contains(searchTerm)))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<bool> IsBarcodeUniqueAsync(string barcode, int? excludeId = null)
    {
        var query = _context.Products.Where(p => p.Barcode == barcode && p.IsActive);
        
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return !await query.AnyAsync();
    }
}
