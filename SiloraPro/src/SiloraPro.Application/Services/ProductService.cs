namespace SiloraPro.Application.Services;

using SiloraPro.Domain.Entities;
using SiloraPro.Application.Interfaces;

/// <summary>
/// خدمة إدارة المنتجات
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(string name, string description, decimal price, int stockQuantity, string category, string barcode)
    {
        if (await _productRepository.IsBarcodeUniqueAsync(barcode) == false)
            throw new InvalidOperationException("الباركود مستخدم مسبقاً");

        var product = new Product
        {
            Name = name,
            Description = description,
            Price = price,
            StockQuantity = stockQuantity,
            Category = category,
            Barcode = barcode,
            CreatedAt = DateTime.Now,
            IsActive = true
        };

        return await _productRepository.AddAsync(product);
    }

    public async Task UpdateProductAsync(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        await _productRepository.UpdateAsync(product);
    }

    public async Task DeleteProductAsync(int id)
    {
        await _productRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        return await _productRepository.SearchAsync(searchTerm);
    }
}
