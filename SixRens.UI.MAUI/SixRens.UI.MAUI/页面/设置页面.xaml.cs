using CommunityToolkit.Mvvm.ComponentModel;

namespace SixRens.UI.MAUI.页面;

public partial class 占例页面 : ContentPage
{
	private readonly 占例页面绑定 绑定;
    public 占例页面()
	{
		InitializeComponent();
        this.BindingContext = this.绑定 = new 占例页面绑定();
    }

}
public partial class 占例页面绑定 : ObservableObject
{

}
