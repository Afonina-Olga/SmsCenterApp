using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI.Shared.Services;

public interface INavigationService
{
    ViewModelBase CurrentViewModel { get; }
    event Action StateChanged;
    void Navigate(ViewModelBase viewModel);
}