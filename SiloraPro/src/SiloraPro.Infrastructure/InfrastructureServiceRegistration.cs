using Microsoft.Extensions.DependencyInjection;
using SiloraPro.Infrastructure.Data;
using SiloraPro.Infrastructure.Repositories;
using SiloraPro.Domain.Entities;
using SiloraPro.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SiloraPro.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString = "Data Source=silorapro.db")
    {
        // تسجيل سياق قاعدة البيانات
        services.AddDbContext<SiloraProDbContext>(options =>
            options.UseSqlite(connectionString));

        // تسجيل المستودعات (Repositories)
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        
        return services;
    }
}
