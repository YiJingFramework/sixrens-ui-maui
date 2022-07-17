using CommunityToolkit.Mvvm.ComponentModel;

namespace SixRens.UI.MAUI.页面;

public partial class 设置页面 : ContentPage
{
	private readonly 设置页面绑定 绑定;
    public 设置页面()
	{
		InitializeComponent();
        this.BindingContext = this.绑定 = new 设置页面绑定();
    }

}
public partial class 设置页面绑定 : ObservableObject
{

}
