using SmsCenter.UI.Shared.Commands;

namespace SmsCenter.UI.Pages.GetCost;

public class AddPhoneNumberCommand(AddPhoneNumberViewModel viewModel) : CommandBase
{
    public override bool CanExecute(object? parameter) => viewModel.CanSave;

    public override void Execute(object? parameter)
    {
        throw new NotImplementedException();
    }
}