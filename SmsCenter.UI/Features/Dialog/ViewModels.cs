using System.Diagnostics;
using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI.Features.Dialog;

public class DialogViewModel : ViewModelBase
{
    public string Header { get; set; } = default!;
    public ViewModelBase CurrentViewModel { get; set; } = default!;
}

public class DialogSettings<TModel>
    where TModel : ViewModelBase
{
    public string Header { get; set; } = default!;
    public string ActionButtonName { get; set; } = default!;
    public string CancelButtonName { get; set; } = default!;
    public Action<TModel> Init { get; set; } = default!;
}