using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SmsCenter.UI.Shared.ViewModels;

public class ValidationViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<ValidationResult>> _errors = new();

    public bool HasErrors => _errors.Count is not 0;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        return (!string.IsNullOrEmpty(propertyName)
            ? _errors!.GetValueOrDefault(propertyName, null)
            : [])!;
    }

    public bool FindErrors(string propertyName) => _errors.ContainsKey(propertyName);

    protected void ValidateProperty<T>(T value, [CallerMemberName] string propertyName = null)
    {
        if (string.IsNullOrEmpty(propertyName))
            return;

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateProperty(
            value,
            new ValidationContext(this)
            {
                MemberName = propertyName
            },
            validationResults);

        if (isValid)
        {
            _errors.Remove(propertyName);
        }
        else
        {
            _errors[propertyName] = validationResults;
        }

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    protected void ClearErrors(string propertyName) => _errors.Remove(propertyName);

    public void ClearErrors() => _errors.Clear();
}