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
            await DetectShowInstallDefaultPopupsAsync();
        }

        private async Task DetectShowInstallDefaultPopupsAsync()
        {
            var withoutPlugins = core.PluginPackageManager.插件包.Count is 0;
             var withoutPresets = core.PresetManager.预设列表.Count is 0;
            if (withoutPlugins)
            {
                if (withoutPresets)
                {
                    // 两者都要
                    var install = await DisplayAlert(
                        "自动配置",
                        "检测到您没有配置任何插件包和预设，是否安装默认配置？",
                        "安装", "稍后");
                    if (install)
                    {
                        _ = await core.InstallDefaultPluginsAsync();
                        await core.InstallDefaultPresetsAsync();
                    }
                }
                else
                {
                    // 仅插件包
                    var install = await DisplayAlert(
                        "自动配置",
                        "检测到您没有任何插件包，是否安装默认插件包？",
                        "安装", "稍后");
                    if (install)
                        _ = await core.InstallDefaultPluginsAsync();
                }
            }
            else if (withoutPresets)
            {
                // 仅预设
                var install = await DisplayAlert(
                    "自动配置",
                    "检测到您没有任何预设，是否导入默认预设？",
                    "导入", "稍后");
                if (install)
                    await core.InstallDefaultPresetsAsync();
            }
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