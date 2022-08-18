namespace SixRens.UI.MAUI.Views;

public partial class CaseManagementPage : ContentPage
{
    AppShell shell;
    public CaseManagementPage(AppShell shell)
    {
        InitializeComponent();
        this.shell = shell;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        shell.GoToAsync(new ShellNavigationState("//settings"), new Dictionary<string, object>() {
            ["text"] = "hello settings! i'm cm"
        });
    }
}