using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using SmsCenter.UI.Shared.Commands;
using SmsCenter.UI.Shared.Controls;
using SmsCenter.UI.Shared.Services;

namespace SmsCenter.UI.Shared.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ViewModelBase CurrentViewModel => _navigationService.CurrentViewModel;

        public MenuViewModel Menu { get; set; }

        // ReSharper disable once MemberCanBePrivate.Global
        public ICommand NavigateCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _navigationService.StateChanged += OnNavigatorStateChanged;
            NavigateCommand = new NavigateCommand(navigationService);
            NavigateCommand.Execute(ViewType.SendSms);
            var menu = App.Current.Services.GetRequiredService<MenuViewModel>();

            menu.MenuItems.Add(CreateItem(1, true, "Получить стоимость", ViewType.SendSms, PackIconKind.CurrencyRub));
            menu.MenuItems.Add(CreateItem(2, false, "Отправить смс", ViewType.SendSms, PackIconKind.MessageAlert));
            menu.MenuItems.Add(CreateItem(3, false, "Узнать баланс", ViewType.SendSms, PackIconKind.Wallet));
            menu.MenuItems.Add(CreateItem(4, false, "Статистика", ViewType.SendSms, PackIconKind.ChartPie));
            Menu = menu;
        }

        private void OnNavigatorStateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public override void Dispose()
        {
            _navigationService.StateChanged -= OnNavigatorStateChanged;
            base.Dispose();
        }

        private static MenuItemViewModel CreateItem(
            int id,
            bool isActive,
            string name,
            ViewType type, PackIconKind icon)
        {
            var model = App.Current.Services.GetRequiredService<MenuItemViewModel>();
            model.Id = id;
            model.IsActive = isActive;
            model.Name = name;
            model.Type = type;
            model.Icon = icon;
            return model;
        }
    }
}