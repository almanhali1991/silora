using Microsoft.Extensions.DependencyInjection;
using SiloraPro.Infrastructure.Data;
using SiloraPro.Infrastructure.Repositories;
using SiloraPro.Domain.Entities;
using SiloraPro.Application.Interfaces;

namespace SiloraPro.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // تسجيل سياق قاعدة البيانات
        services.AddDbContext<AppDbContext>();

        // تسجيل المستودعات (Repositories)
        services.AddScoped<IRepository<Customer>, Repository<Customer>>();
        services.AddScoped<IRepository<Product>, Repository<Product>>();
        services.AddScoped<IRepository<Order>, Repository<Order>>();
        
        // تسجيل واجهات الخدمات إذا كانت موجودة في البنية التحتية (عادة تكون في Application)
        // لكن سنتركها للتطبيق لتجنب التداخل
        
        return services;
    }
}
