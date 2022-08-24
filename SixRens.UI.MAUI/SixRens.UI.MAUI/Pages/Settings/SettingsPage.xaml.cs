using SixRens.UI.MAUI.Tools.Extensions;
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
        label.Text = query.GetValueOrDefault("text", "") as string;
    }
}