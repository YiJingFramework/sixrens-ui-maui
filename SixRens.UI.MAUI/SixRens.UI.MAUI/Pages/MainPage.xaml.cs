using AndroidX.Lifecycle;
using SixRens.UI.MAUI.Services.ExceptionHandling;
using SixRens.UI.MAUI.Services.SixRens;
using static Android.App.Assist.AssistStructure;

namespace SixRens.UI.MAUI.Pages
{
    public partial class MainPage : ContentPage
    {
        private bool firstLoad;
        private SixRensCore core;
        private readonly ExceptionHandler exceptionHandler;
        public MainPage(
            SixRensCore core,
            ExceptionHandler exceptionHandler)
        {
            this.core = core;
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
    }
}