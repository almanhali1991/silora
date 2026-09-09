using Microsoft.Extensions.DependencyInjection;
using SiloraPro.Application.Interfaces;
using SiloraPro.Application.Services;

namespace SiloraPro.Application;

/// <summary>
/// فئة تسجيل خدمات التطبيق
/// </summary>
public static class ApplicationRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // تسجيل الخدمات
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
