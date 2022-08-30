using AndroidX.Lifecycle;
using SixRens.Core.占例存取;
using SixRens.Core.壬式生成;
using SixRens.UI.MAUI.Services.ExceptionHandling;
using SixRens.UI.MAUI.Services.SixRens;
using SixRens.UI.MAUI.Tools.Extensions;
using SixRens.UI.MAUI.Tools.Querying;
using System.Diagnostics;
using static Android.App.Assist.AssistStructure;

namespace SixRens.UI.MAUI.Pages.Main
{
    public partial class InstallDefaultsPromptPage : ContentPage
    {
        private readonly AppShell shell;

        [Flags]
        private enum PromptModes
        {
            None = 0b00,
            WithoutPlugins = 0b01,
            WithoutPresets = 0b10,
            WithoutBoth = 0b11
        }

        internal static InstallDefaultsPromptPage CreatePageIfRequired(
            SixRensCore core, AppShell shell)
        {
            var withoutPlugins = core.PluginPackageManager.插件包.Count is 0;
            var withoutPresets = core.PresetManager.预设列表.Count is 0;

            var mode = PromptModes.None;
            if (withoutPlugins)
                mode |= PromptModes.WithoutPlugins;
            if (withoutPresets)
                mode |= PromptModes.WithoutPresets;
            if (mode is PromptModes.None)
                return null;
            return new(mode, core, shell);
        }

        private InstallDefaultsPromptPage(
            PromptModes mode,
            SixRensCore core,
            AppShell shell)
        {
            this.shell = shell;
            InitializeComponent();

            switch (mode)
            {
                case PromptModes.WithoutPlugins:
                {
                    this.label.Text = "检测到您没有任何插件包，是否安装默认插件包？";
                    this.button.Text = "安装";
                    this.installAction = async () => {
                        var result = await core.InstallDefaultPluginsAsync();
                        Debug.Assert(result is true);
                    };
                    break;
                }
                case PromptModes.WithoutPresets:
                {
                    this.label.Text = "检测到您没有任何预设，是否导入默认预设？";
                    this.button.Text = "导入";
                    this.installAction = async () => {
                        await core.InstallDefaultPresetsAsync();
                    };
                    break;
                }
                case PromptModes.WithoutBoth:
                {
                    this.label.Text = "检测到您没有配置任何插件包和预设，是否安装默认配置？";
                    this.button.Text = "安装";
                    this.installAction = async () => {
                        var result = await core.InstallDefaultPluginsAsync();
                        Debug.Assert(result is true);
                        await core.InstallDefaultPresetsAsync();
                    };
                    break;
                }
                default:
                {
                    Debug.Fail("若非以上三种情况，则不应该构造此界面。");
                    installAction = () => Task.CompletedTask;
                    break;
                }
            }
        }

        private readonly Func<Task> installAction;
        private async void InstallButtonClicked(object sender, EventArgs e)
        {
            await installAction();
            _ = await shell.Navigation.PopAsync();
        }
    }
}