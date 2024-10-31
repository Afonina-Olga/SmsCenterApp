namespace SmsCenter.UI.Shared.Services;

public interface IEventNotificationService
{
    event Action<int> ActiveMenuItemChanged;
    void SetActiveMenuItem(int id);
}

public class EventNotificationService : IEventNotificationService
{
    public event Action<int>? ActiveMenuItemChanged;
    public void SetActiveMenuItem(int id) => ActiveMenuItemChanged?.Invoke(id);
}