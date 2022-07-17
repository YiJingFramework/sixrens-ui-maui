using CommunityToolkit.Mvvm.ComponentModel;

namespace SixRens.UI.MAUI.页面;

public partial class 盘面页面 : ContentPage
{
	private readonly 盘面页面绑定 绑定;
    public 盘面页面()
	{
		InitializeComponent();
        this.BindingContext = this.绑定 = new 盘面页面绑定();
    }
}
public partial class 盘面页面绑定 : ObservableObject
{

}
