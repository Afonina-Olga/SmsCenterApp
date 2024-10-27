using SmsCenter.UI.Shared.Commands;
using SmsCenter.UI.Shared.Notifications;
using SmsCenter.UI.Shared.Services;

namespace SmsCenter.UI.Shared.Controls.Menu;

public class MenuCommand(MenuItemViewModel viewModel, INavigationService service) : CommandBaseAsync
{
    protected override Task ExecuteAsync(object parameter)
    {
        try
        {
            new NavigateCommand(service).Execute(viewModel.Type);
            viewModel.IsActive = true;
        }
        catch (Exception e)
        {
            PopupNotification.ShowError("Не удалось загрузить страницу", e.Message);
        }

        return Task.CompletedTask;
    }
}