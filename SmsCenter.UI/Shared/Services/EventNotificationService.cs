namespace SmsCenter.UI.Shared.Services;

public class EventNotificationService : IEventNotificationService
{
    public event Action<int>? ActiveMenuItemChanged;
    public void SetActiveMenuItem(int id) => ActiveMenuItemChanged?.Invoke(id);
}