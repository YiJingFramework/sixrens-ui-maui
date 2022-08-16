using SixRens.UI.MAUI.Services.ExceptionHandling;

namespace SixRens.UI.MAUI
{
    public partial class App : Application
    {
        public App(AppShell shell, ExceptionHandler exceptionHandler)
        {
            InitializeComponent();
            MainPage = shell;

            AppDomain.CurrentDomain.UnhandledException +=
                (_, e) => exceptionHandler.Handle(
                    (Exception)e.ExceptionObject, true, false, false);
        }
    }
}