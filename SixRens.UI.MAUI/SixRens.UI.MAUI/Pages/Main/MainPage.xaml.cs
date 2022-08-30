using AndroidX.Lifecycle;
using SixRens.Core.占例存取;
using SixRens.Core.壬式生成;
using SixRens.UI.MAUI.Services.ExceptionHandling;
using SixRens.UI.MAUI.Services.SixRens;
using SixRens.UI.MAUI.Tools.Extensions;
using SixRens.UI.MAUI.Tools.Querying;
using static Android.App.Assist.AssistStructure;

namespace SixRens.UI.MAUI.Pages.Main
{
    public partial class MainPage : ContentPage, IQueryAttributable<MainPageQueryParameters>
    {
        private bool firstLoad;
        private readonly SixRensCore core;
        private readonly AppShell shell;
        private readonly ExceptionHandler exceptionHandler;
        public MainPage(
            SixRensCore core,
            AppShell shell,
            ExceptionHandler exceptionHandler)
        {
            this.core = core;
            this.shell = shell;
            this.exceptionHandler = exceptionHandler;

            firstLoad = true;
            Loaded += async (_, _) => await OnLoaded();
            
            InitializeComponent();
        }

        private async Task OnLoaded()
        {
            if (!firstLoad)
                return;
            firstLoad = false;

            await exceptionHandler.SetDisplayPageAsync(this);

            var promptPage = InstallDefaultsPromptPage.CreatePageIfRequired(core, shell);
            if (promptPage is not null)
                await shell.Navigation.PushAsync(promptPage);
        }

        void IQueryAttributable<MainPageQueryParameters>.ApplyQueryParameter(
            MainPageQueryParameters parameter)
        {
            if (parameter is not null)
            {
                this.caseShowingGrid.BindingContext
                    = new BindingCase(parameter.CaseName, parameter.Case);
                
                this.guidingGrid.IsVisible = false;
                this.caseShowingGrid.IsVisible = true;
            }
            else
            {
                this.guidingGrid.IsVisible = true;
                this.caseShowingGrid.IsVisible = false;

                this.caseShowingGrid.BindingContext = null;
            }
        }

        private async void GotoDivinationCreationPage(object sender, EventArgs e)
        {
            await shell.GoToAsync("//new");
        }
    }
}