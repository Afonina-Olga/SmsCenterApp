using SmsCenter.UI.Shared.Commands;
using SmsCenter.UI.Shared.Notifications;
using SmsCenter.UI.Shared.Services;

namespace SmsCenter.UI.Shared.Controls;

public class MenuCommand(
    MenuItemViewModel viewModel,
    INavigationService service,
    IEventNotificationService eventService)
    : CommandBase
{
    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
        try
        {
            new NavigateCommand(service).Execute(viewModel.Type);
            viewModel.IsActive = true;
            eventService.SetActiveMenuItem(viewModel.Id);
        }
        catch (Exception e)
        {
            PopupNotification.ShowError("Не удалось загрузить страницу", e.Message);
        }
    }
}