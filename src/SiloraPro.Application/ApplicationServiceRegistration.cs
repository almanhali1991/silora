using Microsoft.Extensions.DependencyInjection;
using SiloraPro.Application.Interfaces;
using SiloraPro.Application.Services;

namespace SiloraPro.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        
        return services;
    }
}
