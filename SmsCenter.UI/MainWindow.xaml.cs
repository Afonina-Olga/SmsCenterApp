using System.Windows;

namespace SmsCenter.UI;

public partial class MainWindow : Window
{
    public MainWindow(object dataContext)
    {
        InitializeComponent();
        DataContext = dataContext;
    }

    private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
    {
        MenuCard.Visibility = MenuCard.IsVisible ? Visibility.Collapsed : Visibility.Visible;
    }
}