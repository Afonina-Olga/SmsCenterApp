using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmsCenter.UI.Pages.GetCost;
using SmsCenter.UI.Pages.SendSms;
using SmsCenter.UI.Shared.Controls;
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
                services.AddTransient<MainViewModel>();
                services.AddTransient<SendSmsViewModel>();
                services.AddTransient<GetCostViewModel>();
                services.AddTransient<MenuItemViewModel>();
                services.AddTransient<MenuViewModel>();

                services.AddSingleton(s =>
                    new MainWindow(s.GetRequiredService<MainViewModel>()));

                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IEventNotificationService, EventNotificationService>();

            });
            return hostBuilder;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            var window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
	}
}
