using SixRens.UI.MAUI.Services.ExceptionHandling;
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
            BindingContext = viewModel;
            Loaded += (_, _) => exceptionHandler.SetDisplayPage(this);
        }
    }
}