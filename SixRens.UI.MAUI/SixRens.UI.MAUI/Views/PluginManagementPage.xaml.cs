using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Storage;
using SixRens.Core.插件管理.插件包管理;
using SixRens.UI.MAUI.Extensions;
using SixRens.UI.MAUI.ViewModels;
using static Android.Graphics.Path;

namespace SixRens.UI.MAUI.Views;

public partial class PluginManagementPage
    : ContentPage, IWithBindingContext<PluginManagementPageViewModel>
{
    private IFilePicker filePicker;
    public PluginManagementPage(PluginManagementPageViewModel viewModel, IFilePicker filePicker)
    {
        this.filePicker = filePicker;
        BindingContext = viewModel;

        InitializeComponent();
    }

    private async void ShowPluginPackageDetails(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var package = (插件包)frame.BindingContext;
        var popupResult = (PluginPackageDetailsPopup.PopupResult)
            await this.ShowPopupAsync(new PluginPackageDetailsPopup(package));
        switch (popupResult)
        {
            case PluginPackageDetailsPopup.PopupResult.DeletionRequired:
            {
                var viewModel = this.GetBindingContext();
                viewModel.RemovePluginPackageCommand.CheckAndExecute(package);
                break;
            }
        }
    }
}