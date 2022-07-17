namespace SixRens.UI.MAUI.页面.盘面控件;

public partial class 未起盘 : ContentView
{
	public 未起盘()
	{
		InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///" + nameof(起课页面));
    }
}