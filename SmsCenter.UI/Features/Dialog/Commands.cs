using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SmsCenter.UI.Shared.Commands;
using SmsCenter.UI.Shared.Services;
using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI.Features.Dialog;

public interface ISaveDialogCommand
{
    public ICommand SaveCommand { get; }
}

public class OpenCreateDialogCommand<TModel>(
    DialogSettings<TModel> settings,
    IDialogService dialogService)
    : CommandBaseAsync
    where TModel : ViewModelBase, ISaveDialogCommand
{
    protected override async Task ExecuteAsync(object parameter)
    {
        var model = App.Current.Services.GetRequiredService<TModel>();
        settings.Init?.Invoke(model);

        var dialog = new DialogViewModel
        {
            Header = settings.Header,
            ActionButtonName = settings.ActionButtonName,
            CancelButtonName = settings.CancelButtonName,
            CurrentViewModel = model,
            ActionCommand = model.SaveCommand
        };
            
        if (string.IsNullOrEmpty(settings.Identifier))
        {
            await dialogService.Show(dialog);
        }
        else
        {
            await dialogService.Show(dialog, settings.Identifier);
        }
    }
}