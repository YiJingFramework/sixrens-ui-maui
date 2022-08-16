namespace SixRens.UI.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            this.CurrentItem = mainPage;
        }
    }
}