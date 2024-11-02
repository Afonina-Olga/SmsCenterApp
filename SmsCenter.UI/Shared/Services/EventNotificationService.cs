namespace SmsCenter.UI.Shared.Services;

public interface IEventNotificationService
{
    event Action<int> ActiveMenuItemChanged;
    event Action<string> PhoneNumberAdded;
    event Action<string> PhoneNumberDeleted;
    void SetActiveMenuItem(int id);
    void AddPhoneNumber(string phoneNumber);
    void DeletePhoneNumber(string phoneNumber);
}

public class EventNotificationService : IEventNotificationService
{
    public event Action<int>? ActiveMenuItemChanged;
    public event Action<string>? PhoneNumberAdded;
    public event Action<string>? PhoneNumberDeleted;
    public void SetActiveMenuItem(int id) => ActiveMenuItemChanged?.Invoke(id);
    public void AddPhoneNumber(string phoneNumber) => PhoneNumberAdded?.Invoke(phoneNumber);
    public void DeletePhoneNumber(string phoneNumber) => PhoneNumberDeleted?.Invoke(phoneNumber);
}