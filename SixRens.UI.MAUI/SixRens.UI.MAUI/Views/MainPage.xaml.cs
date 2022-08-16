using SixRens.UI.MAUI.ViewModels;

namespace SixRens.UI.MAUI.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MainPageViewModel();
        }
    }
}