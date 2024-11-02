using System.ComponentModel;
using System.Text.Json;
using System.Windows;
using SmsCenter.Api.Services;
using SmsCenter.UI.Shared.Commands;
using SmsCenter.UI.Shared.Notifications;
using SmsCenter.UI.Shared.Services;

namespace SmsCenter.UI.Pages.GetCost;

public class AddPhoneNumberCommand(
    AddPhoneNumberViewModel viewModel,
    IEventNotificationService service,
    IDialogService dialogService) : CommandBase
{
    public override bool CanExecute(object? parameter) => viewModel.CanSave;

    public override void Execute(object? parameter)
    {
        service.AddPhoneNumber(viewModel.PhoneNumber);
        dialogService.Close();
    }
}

public class DeletePhoneCommand(
    PhoneViewModel viewModel,
    IEventNotificationService service) : CommandBase
{
    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
        service.DeletePhoneNumber(viewModel.Text);
    }
}

public class SendRequestCommand : CommandBaseAsync
{
    private readonly GetCostViewModel _viewModel;
    private readonly ISmsCenterService _smsCenterService;

    public SendRequestCommand(GetCostViewModel viewModel, ISmsCenterService smsCenterService)
    {
        _viewModel = viewModel;
        _smsCenterService = smsCenterService;
        _viewModel.PropertyChanged += OnPropertyChanged!;
    }

    public override bool CanExecute(object? parameter) => _viewModel.CanExecute;

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GetCostViewModel.CanExecute))
        {
            OnCanExecuteChanged();
        }
    }

    protected override async Task ExecuteAsync(object parameter)
    {
        var phones = _viewModel.PhoneNumbers.Select(x => x.Text).ToArray();
        try
        {
            var result = await _smsCenterService.GetCost(phones, "Привет!");

            var jsonString = JsonSerializer.Serialize(
                result,
                new JsonSerializerOptions { WriteIndented = true });

            PopupNotification.ShowSuccess($"Получен ответ: {Environment.NewLine}{jsonString}");
        }
        catch (Exception e)
        {
            PopupNotification.ShowError(
                $"В процессе отправки запроса произошла ошибка: {Environment.NewLine}{e.Message}");
        }
    }
}