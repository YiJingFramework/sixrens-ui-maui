using Microsoft.Maui.Storage;
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

    private async void PickFileTest(object sender, EventArgs e)
    {
        var t = await filePicker.PickAsync();
    }
}