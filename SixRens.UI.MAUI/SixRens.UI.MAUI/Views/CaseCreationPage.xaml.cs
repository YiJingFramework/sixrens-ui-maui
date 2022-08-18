namespace SixRens.UI.MAUI.Views;

public partial class CaseCreationPage : ContentPage
{
    AppShell shell;
    public CaseCreationPage(AppShell shell)
    {
        InitializeComponent();
        this.shell = shell;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        shell.GoToAsync(new ShellNavigationState("//settings"), new Dictionary<string, object>() {
            ["text"] = "hello settings! i'm cc"
        });
    }
}