using CommunityToolkit.Maui.Views;
using SixRens.Core.插件管理.插件包管理;

namespace SixRens.UI.MAUI.Views;

public partial class PluginPackageDetailsPopup : Popup
{
	public enum PopupResult
	{
		None,
        DeletionRequired
    }
	
	public PluginPackageDetailsPopup(插件包 package)
    {
        this.ResultWhenUserTapsOutsideOfPopup = PopupResult.None;

        this.BindingContext = package;
        InitializeComponent();
    }

    private void ReturnDelete(object sender, EventArgs e)
    {
        this.Close(PopupResult.DeletionRequired);
    }
}