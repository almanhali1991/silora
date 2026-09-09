using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SiloraPro.Application;
using SiloraPro.Infrastructure;
using SiloraPro.Presentation.ViewModels;

namespace SiloraPro.Presentation;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection") 
                    ?? "Data Source=silorapro.db";

                services.AddInfrastructure(connectionString);
                services.AddApplicationServices();
                
                // تسجيل ViewModels
                services.AddTransient<MainViewModel>();
                services.AddTransient<CustomerViewModel>();
            })
            .UseSerilog((context, services, configuration) =>
            {
                configuration.WriteTo.File("logs/silorapro-.log", rollingInterval: RollingInterval.Day);
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();
        
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.DataContext = _host.Services.GetRequiredService<MainViewModel>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }
}
