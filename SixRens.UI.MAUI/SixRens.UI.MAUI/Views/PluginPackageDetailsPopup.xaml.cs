using CommunityToolkit.Maui.Views;
using SixRens.Core.�������.���������;

namespace SixRens.UI.MAUI.Views;

public partial class PluginPackageDetailsPopup : Popup
{
	public enum PopupResult
	{
		None,
        DeletionRequired
    }
	
	public PluginPackageDetailsPopup(����� package)
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