using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmsCenter.UI.Pages.SendSms;
using SmsCenter.UI.Shared.Controls.Menu;
using SmsCenter.UI.Shared.Services;
using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI;

public partial class App : Application
{
    private readonly IHost _host;
    public new static App Current => (App)Application.Current;
    public IServiceProvider Services { get; }

    public App()
    {
        _host = CreateHostBuilder().Build();
        Services = _host.Services;
    }

    private static IHostBuilder CreateHostBuilder(string[]? args = null)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args);
        hostBuilder.ConfigureServices(services =>
        {
            services.AddTransient<MainViewModel>();
            services.AddTransient<MenuItemViewModel>();
            services.AddTransient<MenuViewModel>();
            services.AddTransient<SendSmsViewModel>();

            services.AddSingleton(s =>
                new MainWindow(s.GetRequiredService<MainViewModel>()));

            services.AddSingleton<INavigationService, NavigationService>();
        });
        return hostBuilder;
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            await _host.StartAsync();
            var window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();
            base.OnStartup(e);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"OnStartup: {ex.Message}");
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        try
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"OnExit: {ex.Message}");
        }
    }
}