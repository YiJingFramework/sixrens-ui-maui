using CommunityToolkit.Mvvm.ComponentModel;

namespace SixRens.UI.MAUI.页面;

public partial class 插件页面 : ContentPage
{
	private readonly 插件页面绑定 绑定;
    public 插件页面()
	{
		InitializeComponent();
        this.BindingContext = this.绑定 = new 插件页面绑定();
    }

}
public partial class 插件页面绑定 : ObservableObject
{

}
