using SixRens.UI.MAUI.Tools.Extensions;
using static System.Net.Mime.MediaTypeNames;

namespace SixRens.UI.MAUI.Pages.Settings;

public partial class SettingsPage : ContentPage
{
    private readonly IBrowser browser;
    public SettingsPage(IBrowser browser)
    {
        this.browser = browser;
        InitializeComponent();
    }

    private async void ViewRepositoryClicked(object sender, EventArgs e)
    {
        Uri uri = new("https://github.com/YiJingFramework/sixrens-ui-maui.git");
        BrowserLaunchOptions options = new BrowserLaunchOptions() {
            LaunchMode = BrowserLaunchMode.External
        };
        try
        {
            _ = await browser.OpenAsync(uri, options);
        }
        catch
        { 
            // An unexpected error occured. No browser may be installed on the device.
            // 啥都不做
        }
    }
}