using static System.Net.Mime.MediaTypeNames;

namespace SixRens.UI.MAUI.Pages.Settings;

public partial class SettingsPage : ContentPage, IQueryAttributable
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("text", out var t) && t is string s)
        {
            label.Text = s;
        }
    }
}