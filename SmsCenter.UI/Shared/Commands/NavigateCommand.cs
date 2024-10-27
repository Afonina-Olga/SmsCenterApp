using Microsoft.Extensions.DependencyInjection;
using SmsCenter.UI.Pages.SendSms;
using SmsCenter.UI.Shared.Services;
using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI.Shared.Commands;

public class NavigateCommand : CommandBase
{
    private readonly INavigationService _service;

    public NavigateCommand(INavigationService service)
    {
        _service = service;
    }

    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
        if (parameter is not ViewType viewType) return;
        ViewModelBase viewModel = viewType switch
        {
            ViewType.SendSms => App.Current.Services.GetRequiredService<SendSmsViewModel>(),
            ViewType.Dashboard => App.Current.Services.GetRequiredService<DashboardViewModel>(),
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
                
        _service.Navigate(viewModel);
    }
}