using Microsoft.Extensions.DependencyInjection;
using SiloraPro.Presentation.ViewModels;
using SiloraPro.Presentation.Views;

namespace SiloraPro.Presentation;

/// <summary>
/// فئة تسجيل خدمات واجهة المستخدم
/// </summary>
public static class PresentationRegistration
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        // تسجيل ViewModels
        services.AddSingleton<ViewModelBase>();
        services.AddTransient<CustomerViewModel>();

        // تسجيل النوافذ
        services.AddTransient<MainWindow>();

        return services;
    }
}
