using System.Windows.Media;
using Notification.Wpf;

namespace SmsCenter.UI.Shared.Notifications;

public static class PopupNotification
{
    private static readonly NotificationManager NotificationManager = new();
        
    public static void ShowSuccess(string title)
    {
        var content = new NotificationContent
        {
            Title = title,
            Type = NotificationType.Success,
            TrimType = NotificationTextTrimType.NoTrim,
            RowsCount = 3,
            CloseOnClick = true,
            Foreground = new SolidColorBrush(Colors.Lavender)
        };
        NotificationManager.Show(content);
    }
        
    public static void ShowSuccess(string title, string message)
    {
        var content = new NotificationContent
        {
            Title = title,
            Message = message,
            Type = NotificationType.Success,
            TrimType = NotificationTextTrimType.NoTrim,
            RowsCount = 3,
            CloseOnClick = true,
            Foreground = new SolidColorBrush(Colors.Lavender)
        };
        NotificationManager.Show(content);
    }
        
    public static void ShowError(string title)
    {
        var content = new NotificationContent
        {
            Title = title,
            Type = NotificationType.Error,
            TrimType = NotificationTextTrimType.NoTrim,
            RowsCount = 3,
            CloseOnClick = true,
            Foreground = new SolidColorBrush(Colors.Lavender)
        };
        NotificationManager.Show(content);
    }
        
    public static void ShowError(string title, string message)
    {
        var content = new NotificationContent
        {
            Title = title,
            Message = message,
            Type = NotificationType.Error,
            TrimType = NotificationTextTrimType.NoTrim,
            RowsCount = 3,
            CloseOnClick = true,
            Foreground = new SolidColorBrush(Colors.Lavender)
        };
        NotificationManager.Show(content);
    }
        
    public static void ShowWarning(string title)
    {
        var content = new NotificationContent
        {
            Title = title,
            Type = NotificationType.Warning,
            TrimType = NotificationTextTrimType.NoTrim,
            RowsCount = 3,
            CloseOnClick = true,
            Foreground = new SolidColorBrush(Colors.Lavender)
        };
        NotificationManager.Show(content);
    }
        
    public static void ShowWarning(string title, string message)
    {
        var content = new NotificationContent
        {
            Title = title,
            Message = message,
            Type = NotificationType.Warning,
            TrimType = NotificationTextTrimType.NoTrim,
            RowsCount = 3,
            CloseOnClick = true,
            Foreground = new SolidColorBrush(Colors.Lavender)
        };
        NotificationManager.Show(content);
    }
}