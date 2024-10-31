using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmsCenter.UI.Pages.GetCost;

public partial class AddPhoneView : UserControl
{
    public AddPhoneView()
    {
        InitializeComponent();
    }
    
    private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = PhoneNumberRegex();
        e.Handled = regex.IsMatch(e.Text);
    }

    [GeneratedRegex("[^0-9]+")]
    private static partial Regex PhoneNumberRegex();
}