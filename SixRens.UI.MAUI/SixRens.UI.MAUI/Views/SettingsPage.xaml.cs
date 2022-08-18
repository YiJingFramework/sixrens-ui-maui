using SixRens.UI.MAUI.ViewModels;

namespace SixRens.UI.MAUI.Views;

public partial class SettingsPage : ContentPage, IWithBindingContext<SettingsPageViewModel>
{
    public SettingsPage(SettingsPageViewModel viewModel)
    {
        this.BindingContext = viewModel;
        InitializeComponent();
    }
}