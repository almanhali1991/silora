namespace SiloraPro.Application.Services;

using SiloraPro.Domain.Entities;
using SiloraPro.Application.Interfaces;

/// <summary>
/// خدمة إدارة الطلبات
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task<Order> CreateOrderAsync(int customerId, List<OrderItemDto> items, string notes = "")
    {
        var order = new Order
        {
            OrderNumber = GenerateOrderNumber(),
            CustomerId = customerId,
            OrderDate = DateTime.Now,
            Status = "جديد",
            Notes = notes,
            OrderItems = new List<OrderItem>()
        };

        decimal totalAmount = 0;

        foreach (var itemDto in items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
            if (product == null || !product.IsActive)
                throw new InvalidOperationException($"المنتج غير موجود أو غير نشط: {itemDto.ProductId}");

            if (product.StockQuantity < itemDto.Quantity)
                throw new InvalidOperationException($"الكمية غير متوفرة للمنتج: {product.Name}");

            var orderItem = new OrderItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price,
                TotalPrice = product.Price * itemDto.Quantity
            };

            order.OrderItems.Add(orderItem);
            totalAmount += orderItem.TotalPrice;

            // تحديث المخزون
            product.StockQuantity -= itemDto.Quantity;
            await _productRepository.UpdateAsync(product);
        }

        order.TotalAmount = totalAmount;

        return await _orderRepository.AddAsync(order);
    }

    public async Task UpdateOrderStatusAsync(int orderId, string status)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new InvalidOperationException("الطلب غير موجود");

        order.Status = status;
        await _orderRepository.UpdateAsync(order);
    }

    public async Task DeleteOrderAsync(int orderId)
    {
        await _orderRepository.DeleteAsync(orderId);
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
    {
        return await _orderRepository.GetByCustomerAsync(customerId);
    }

    public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _orderRepository.GetByDateRangeAsync(startDate, endDate);
    }

    private string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }
}
