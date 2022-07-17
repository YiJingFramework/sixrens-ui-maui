using CommunityToolkit.Mvvm.ComponentModel;

namespace SixRens.UI.MAUI.页面;

public partial class 起课页面 : ContentPage
{
    private readonly 起课页面绑定 绑定;
    public 起课页面()
	{
		InitializeComponent();

        this.BindingContext = 绑定 = new 起课页面绑定();
    }
}
public partial class 起课页面绑定 : ObservableObject
{

}