namespace SiloraPro.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiloraPro.Infrastructure.Data;
using SiloraPro.Infrastructure.Repositories;
using SiloraPro.Application.Interfaces;

/// <summary>
/// فئة تسجيل خدمات البنية التحتية
/// </summary>
public static class InfrastructureRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // إعداد قاعدة البيانات SQLite
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Data Source=silorapro.db";

        services.AddDbContext<SiloraProDbContext>(options =>
            options.UseSqlite(connectionString));

        // تسجيل المستودعات
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
