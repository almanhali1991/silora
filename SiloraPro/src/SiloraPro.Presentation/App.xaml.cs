using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Windows;

namespace SiloraPro.Presentation;

/// <summary>
/// فئة التطبيق الرئيسية - نقطة الدخول
/// </summary>
public partial class App : System.Windows.Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // تسجيل الخدمات من الطبقات المختلفة
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection") 
                    ?? "Data Source=silorapro.db";
                
                services.AddInfrastructure(connectionString);
                services.AddApplicationServices();
                services.AddPresentationServices();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }
}
