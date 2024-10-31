using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI.Features.SimpleDialog;

public class SimpleDialogViewModel : ViewModelBase
{
    public string Header { get; set; } = default!;
    public ViewModelBase CurrentViewModel { get; set; } = default!;
}
    
public class SimpleDialogSettings<TModel>
    where TModel : ViewModelBase
{
    public string Header { get; set; } = default!;
    public Action<TModel> Init { get; set; } = default!;
}