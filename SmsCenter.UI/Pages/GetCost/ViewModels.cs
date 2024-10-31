using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using SmsCenter.UI.Features.Dialog;
using SmsCenter.UI.Shared.Services;
using SmsCenter.UI.Shared.ViewModels;

namespace SmsCenter.UI.Pages.GetCost;

public class GetCostViewModel : ViewModelBase
{
    public ICommand OpenDialogCommand { get; }

    public GetCostViewModel(IDialogService dialogService)
    {
        var settings = new DialogSettings<AddPhoneNumberViewModel>
        {
            Header = "Добавить номер телефона",
            ActionButtonName = "Сохранить",
            CancelButtonName = "Отменить"
        };

        OpenDialogCommand = new OpenCreateDialogCommand<AddPhoneNumberViewModel>(settings, dialogService);
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

    public AddPhoneNumberViewModel()
    {
        SaveCommand = new AddPhoneNumberCommand(this);
    }
}