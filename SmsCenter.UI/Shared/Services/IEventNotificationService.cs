namespace SmsCenter.UI.Shared.Services
{
    public interface IEventNotificationService
    {
        event Action<int> ActiveMenuItemChanged;
        void SetActiveMenuItem(int id);
    }
}