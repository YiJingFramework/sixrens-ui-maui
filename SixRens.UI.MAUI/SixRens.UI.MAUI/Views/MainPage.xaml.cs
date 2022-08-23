using AndroidX.Lifecycle;
using SixRens.UI.MAUI.Extensions;
using SixRens.UI.MAUI.Models;
using SixRens.UI.MAUI.Services.ExceptionHandling;
using SixRens.UI.MAUI.ViewModels;
using static Android.App.Assist.AssistStructure;

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

        private async Task OnLoaded()
        {
            if (!firstLoad)
                return;
            firstLoad = false;

            await exceptionHandler.SetDisplayPageAsync(this);
            foreach(var f in functionsToDoAfterFirstLoad)
                await f();
        }


        private List<Func<Task>> functionsToDoAfterFirstLoad = new();
        private async Task DoAsyncAfterFirstLoad(Func<Task> func)
        {
            if (firstLoad)
                functionsToDoAfterFirstLoad.Add(func);
            else
                await func();
        }

        private async void DefaultPluginAndPresetRequirementChanged(
            object sender, Shared.ValueMonitor<DefaultPluginAndPresetRequirement>.ChangedValue e)
        {
            if (e.NewValue.WithoutPlugins)
            {
                if (e.NewValue.WithoutPresets)
                {
                    // 两者都要
                    await DoAsyncAfterFirstLoad(async () => {
                        var install = await DisplayAlert(
                            "自动配置",
                            "检测到您没有配置任何插件包和预设，是否安装默认配置？",
                            "安装", "稍后");
                        if (install)
                        {
                            var viewModel = this.GetBindingContext();
                            viewModel.InstallDefaultPluginsAndPresetsCommand.CheckAndExecute(e.NewValue);
                        }
                    });
                }
                else
                {
                    // 仅插件包
                    await DoAsyncAfterFirstLoad(async () => {
                        var install = await DisplayAlert(
                            "自动配置",
                            "检测到您没有任何插件包，是否安装默认插件包？",
                            "安装", "稍后");
                        if (install)
                        {
                            var viewModel = this.GetBindingContext();
                            viewModel.InstallDefaultPluginsAndPresetsCommand.CheckAndExecute(e.NewValue);
                        }
                    });
                }
            }
            else if (e.NewValue.WithoutPresets)
            {
                // 仅预设
                await DoAsyncAfterFirstLoad(async () => {
                    var install = await DisplayAlert(
                        "自动配置",
                        "检测到您没有任何预设，是否导入默认预设？",
                        "导入", "稍后");
                    if (install)
                    {
                        var viewModel = this.GetBindingContext();
                        viewModel.InstallDefaultPluginsAndPresetsCommand.CheckAndExecute(e.NewValue);
                    }
                });
            }
        }
    }
}