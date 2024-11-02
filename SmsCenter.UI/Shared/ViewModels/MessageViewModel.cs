namespace SmsCenter.UI.Shared.ViewModels;

public class MessageViewModel : ViewModelBase
{
    private string _message = default!;
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged(nameof(Message));
            OnPropertyChanged(nameof(HasMessage));
        }
    }

    public bool HasMessage => !string.IsNullOrEmpty(Message);
}