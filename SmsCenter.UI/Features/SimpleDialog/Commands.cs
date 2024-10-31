using Microsoft.Extensions.DependencyInjection;
using SmsCenter.UI.Shared.Commands;
using SmsCenter.UI.Shared.Services;
using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI.Features.SimpleDialog;

public class OpenViewDialogCommand<TModel>(SimpleDialogSettings<TModel> viewModel, IDialogService dialogService)
    : CommandBaseAsync
    where TModel : ViewModelBase
{
    protected override async Task ExecuteAsync(object parameter)
    {
        var model = App.Current.Services.GetRequiredService<TModel>();
        viewModel.Init?.Invoke(model);

        var dialog = new SimpleDialogViewModel
        {
            Header = viewModel.Header,
            CurrentViewModel = model
        };
        
        await dialogService.Show(dialog);
    }
}