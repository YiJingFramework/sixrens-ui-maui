using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SixRens.UI.MAUI.Services.SixRens;

namespace SixRens.UI.MAUI.ViewModels
{
    public sealed partial class MainPageViewModel : ObservableObject
    {
        private readonly SixRensCore core;
        public MainPageViewModel(SixRensCore core)
        {
            this.core = core;

            DisplayNoPluginPrompt = core.PluginPackageManager.插件包.Count is 0;
            DisplayNoPresetPrompt = core.PresetManager.预设列表.Count is 0;
        }

        public bool DisplayNoPluginPrompt { get; }

        public bool DisplayNoPresetPrompt { get; }

        [RelayCommand]
        private async Task InstallDefaultPluginsAsync()
        {
            _ = await core.InstallDefaultPluginsAsync();
        }

        [RelayCommand]
        private async Task InstallDefaultPresetsAsync()
        {
            await core.InstallDefaultPresetsAsync();
        }
    }
}
