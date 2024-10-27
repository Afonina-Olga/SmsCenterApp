using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using SmsCenter.UI.Shared.Commands;
using SmsCenter.UI.Shared.Controls.Menu;
using SmsCenter.UI.Shared.Services;

namespace SmsCenter.UI.Shared.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    public ViewModelBase CurrentViewModel => _navigationService.CurrentViewModel;

    public MenuViewModel Menu { get; set; } 

    // ReSharper disable once MemberCanBePrivate.Global
    public ICommand NavigateCommand { get; }

    public MainViewModel(INavigationService navigationService)
    {
        // _navigationService = navigationService;
        //
        // _navigationService.StateChanged += OnNavigatorStateChanged;
        //
        // NavigateCommand = new NavigateCommand(navigationService);
        // NavigateCommand.Execute(ViewType.SendSms);
        var menu = App.Current.Services.GetRequiredService<MenuViewModel>();

        menu.MenuItems.Add(CreateItem(
            id: 1,
            isActive: true,
            name: "Отправка смс",
            type: ViewType.SendSms,
            icon: PackIconKind.ClipboardListOutline));
        
        menu.MenuItems.Add(CreateItem(
            id: 2,
            isActive: false,
            name: "Статус смс",
            type: ViewType.StatusSms,
            icon: PackIconKind.FaceAgent));

        Menu = menu;
    }

    private void OnNavigatorStateChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    public override void Dispose()
    {
       // _navigationService.StateChanged -= OnNavigatorStateChanged;
        base.Dispose();
    }

    private static MenuItemViewModel CreateItem(int id, bool isActive, string name, ViewType type, PackIconKind icon)
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