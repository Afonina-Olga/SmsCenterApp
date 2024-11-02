using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmsCenter.Api;
using SmsCenter.UI.Pages.GetCost;
using SmsCenter.UI.Pages.SendSms;
using SmsCenter.UI.Shared.Controls;
using SmsCenter.UI.Shared.Notifications;
using SmsCenter.UI.Shared.Services;
using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
                services.AddSingleton<MainViewModel>();
                services.AddTransient<SendSmsViewModel>();
                services.AddSingleton<GetCostViewModel>();
                services.AddTransient<MenuItemViewModel>();
                services.AddTransient<AddPhoneNumberViewModel>();
                services.AddTransient<MenuViewModel>();

                services.AddSingleton(s =>
                    new MainWindow(s.GetRequiredService<MainViewModel>()));

                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IEventNotificationService, EventNotificationService>();
                services.AddSingleton<IDialogService, DialogService>();

                var projectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var filePath = Directory.GetParent(projectPath!)!.Parent!.Parent!.FullName;
                var builder = new ConfigurationBuilder()
                    .AddJsonFile(
                        Path.Combine(filePath, "appsettings.json"),
                        true,
                        true);
                services.AddSmscService(builder.Build());
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
                PopupNotification.ShowError($"Startup exception: {ex.Message}");
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
                PopupNotification.ShowError($"Exit exception: {ex.Message}");
            }
        }
    }
}