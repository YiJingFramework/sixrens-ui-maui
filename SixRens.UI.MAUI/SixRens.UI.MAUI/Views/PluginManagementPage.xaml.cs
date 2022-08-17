using SixRens.UI.MAUI.ViewModels;

namespace SixRens.UI.MAUI.Views;

public partial class PluginManagementPage
    : ContentPage, IWithBindingContext<PluginManagementPageViewModel>
{
    public PluginManagementPage(PluginManagementPageViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}