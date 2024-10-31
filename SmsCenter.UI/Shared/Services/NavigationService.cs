using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI.Shared.Services;

public interface INavigationService
{
    ViewModelBase CurrentViewModel { get; }
    event Action StateChanged;
    void Navigate(ViewModelBase viewModel);
}

public class NavigationService : INavigationService
{
    public ViewModelBase CurrentViewModel { get; private set; } = default!;
    public event Action? StateChanged;

    public void Navigate(ViewModelBase viewModel)
    {
        CurrentViewModel?.Dispose();
        CurrentViewModel = viewModel;
        StateChanged?.Invoke();
    }
}