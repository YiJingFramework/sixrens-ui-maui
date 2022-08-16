using SixRens.UI.MAUI.Services.ExceptionHandling;
using SixRens.UI.MAUI.Services.SixRens;
using SixRens.UI.MAUI.ViewModels;

namespace SixRens.UI.MAUI.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(
            MainPageViewModel viewModel,
            ExceptionHandler exceptionHandler)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
            this.Loaded += (_, _) => exceptionHandler.SetDisplayPage(this);
        }
    }
}