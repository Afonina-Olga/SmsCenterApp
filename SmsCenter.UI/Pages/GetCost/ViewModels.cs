using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using SmsCenter.Api.Services;
using SmsCenter.UI.Features.Dialog;
using SmsCenter.UI.Shared.Notifications;
using SmsCenter.UI.Shared.Services;
using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI.Pages.GetCost;

public class PhoneViewModel : ViewModelBase
{
    public string Text { get; set; } = default!;
    public ICommand DeleteCommand { get; set; }

    public PhoneViewModel(IEventNotificationService service)
    {
        DeleteCommand = new DeletePhoneCommand(this, service);
    }
}

public class GetCostViewModel : ValidationViewModel
{
    private readonly IEventNotificationService _notificationService;
    private ObservableCollection<PhoneViewModel> _phoneNumbers = [];

    public ObservableCollection<PhoneViewModel> PhoneNumbers
    {
        get => _phoneNumbers;
        set
        {
            _phoneNumbers = value;
            OnPropertyChanged();
        }
    }

    private string _message = default!;

    [Required(ErrorMessage = "Не заполнено")]
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            ValidateProperty(value);
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanExecute));
        }
    }
    
    public ICommand OpenDialogCommand { get; }
    public ICommand SendRequestCommand { get; }

    public bool CanExecute => PhoneNumbers.Any() && !string.IsNullOrEmpty(Message);

    public GetCostViewModel(
        IDialogService dialogService,
        IEventNotificationService notificationService,
        ISmsCenterService smsCenterService)
    {
        _notificationService = notificationService;
        _notificationService.PhoneNumberAdded += OnPhoneNumberAdded;
        _notificationService.PhoneNumberDeleted += OnPhoneNumberDeleted;

        var settings = new DialogSettings<AddPhoneNumberViewModel>
        {
            Header = "Добавить номер телефона",
            ActionButtonName = "Сохранить",
            CancelButtonName = "Отменить"
        };

        OpenDialogCommand = new OpenCreateDialogCommand<AddPhoneNumberViewModel>(settings, dialogService);
        SendRequestCommand = new SendRequestCommand(this, smsCenterService);
    }

    private void OnPhoneNumberDeleted(string phoneNumber)
    {
        var result = PhoneNumbers.FirstOrDefault(x => x.Text == phoneNumber);
        PhoneNumbers.Remove(result!);
        
        OnPropertyChanged(nameof(CanExecute));
    }

    private void OnPhoneNumberAdded(string phoneNumber)
    {
        var normalizedPhoneNumber = $"+7{phoneNumber}";
        if (PhoneNumbers.Select(x => x.Text).Contains(normalizedPhoneNumber))
        {
            PopupNotification.ShowError("Такой номер телефона уже добавлен");
            return;
        }

        PhoneNumbers.Add(new PhoneViewModel(_notificationService)
        {
            Text = normalizedPhoneNumber
        });
        
        OnPropertyChanged(nameof(CanExecute));
    }

    // Для singletone dispose не нужен
    public override void Dispose()
    {
        // _notificationService.PhoneNumberDeleted -= OnPhoneNumberDeleted;
        // _notificationService.PhoneNumberAdded -= OnPhoneNumberAdded;
        base.Dispose();
    }
}

public class AddPhoneNumberViewModel : ValidationViewModel, ISaveDialogCommand
{
    private string _phoneNumber = default!;

    [Required(ErrorMessage = "Не заполнено")]
    [RegularExpression("\\d{10}", ErrorMessage = "Неверный формат номера")]
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            ValidateProperty(value);
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSave));
        }
    }

    public bool CanSave => !HasErrors && string.IsNullOrEmpty(PhoneNumber) is false;

    public ICommand SaveCommand { get; }

    public AddPhoneNumberViewModel(IEventNotificationService notificationService, IDialogService dialogService)
    {
        SaveCommand = new AddPhoneNumberCommand(this, notificationService, dialogService);
    }
}