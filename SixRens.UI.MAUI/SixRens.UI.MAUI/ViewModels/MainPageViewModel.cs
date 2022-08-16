using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SixRens.UI.MAUI.Services.SixRens;

namespace SixRens.UI.MAUI.ViewModels
{
    public sealed partial class MainPageViewModel : ObservableObject
    {
        private SixRensCore core;
        public MainPageViewModel(SixRensCore core)
        {
            this.core = core;
            PluginsCount = core.PluginPackageManager.插件包.Count;
            PresetsCount = core.PresetManager.预设列表.Count;
        }

        [ObservableProperty]
        private int pluginsCount;

        [RelayCommand]
        private async Task InstallDefaultPlugins()
        {
            await core.InstallDefaultPlugins();
            PluginsCount = core.PluginPackageManager.插件包.Count;
        }

        [ObservableProperty]
        private int presetsCount;

        [RelayCommand]
        private async Task InstallDefaultPresets()
        {
            await core.InstallDefaultPresets();
            PresetsCount = core.PresetManager.预设列表.Count;
        }
    }
}
