using SixRens.UI.MAUI.Extensions;
using SixRens.UI.MAUI.Services.ExceptionHandling;
using SixRens.UI.MAUI.ViewModels;

namespace SixRens.UI.MAUI.Views
{
    public partial class MainPage : ContentPage, IWithBindingContext<MainPageViewModel>
    {
        private bool firstLoad;
        private readonly ExceptionHandler exceptionHandler;
        public MainPage(
            MainPageViewModel viewModel,
            ExceptionHandler exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler;

            firstLoad = true;
            Loaded += async (_, _) => await OnLoaded();

            BindingContext = viewModel;
            InitializeComponent();
        }

        private async Task InstallDefaults()
        {
            var viewModel = this.GetBindingContext();
            if (viewModel.DisplayNoPluginPrompt)
            {
                if (viewModel.DisplayNoPresetPrompt)
                {
                    // 两者都要
                    var install = await DisplayAlert(
                        "自动配置",
                        "检测到您没有配置任何插件包和预设，是否安装默认配置？",
                        "安装", "稍后");
                    if (install)
                    {
                        viewModel.InstallDefaultPluginsCommand.CheckAndExecute();
                        viewModel.InstallDefaultPresetsCommand.CheckAndExecute();
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
                        viewModel.InstallDefaultPluginsCommand.CheckAndExecute();
                }
            }
            else if (viewModel.DisplayNoPresetPrompt)
            {
                // 仅预设
                var install = await DisplayAlert(
                    "自动配置",
                    "检测到您没有任何预设，是否导入默认预设？",
                    "导入", "稍后");
                if (install)
                    viewModel.InstallDefaultPresetsCommand.CheckAndExecute();
            }
        }

        private async Task OnLoaded()
        {
            if (!firstLoad)
                return;
            firstLoad = false;

            await exceptionHandler.SetDisplayPageAsync(this);
            await InstallDefaults();
        }
    }
}