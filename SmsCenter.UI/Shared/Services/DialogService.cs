using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using SmsCenter.UI.Shared.Notifications;
using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI.Shared.Services;

public interface IDialogService
{
    Task Show<TModel>(TModel content, string identifier = "RootDialogHost") where TModel : ViewModelBase;
    void Close(string identifier = "RootDialogHost");
}

public class DialogService : IDialogService
{
    public async Task Show<TModel>(TModel content, string identifier = "RootDialogHost") 
        where TModel : ViewModelBase
    {
        try
        {
            var model = content ?? App.Current.Services.GetRequiredService<TModel>();
            await DialogHost.Show(model, identifier);
        }
        catch (Exception e)
        {
            PopupNotification.ShowError("Не удалось открыть окно", e.Message);
        }
    }

    public void Close(string identifier = "RootDialogHost")
    {
        try
        {
            DialogHost.Close(identifier);
        }
        catch (Exception e)
        {
            PopupNotification.ShowError($"Не удалось закрыть окно (identifier: {identifier})", e.Message);
        }
    }
}