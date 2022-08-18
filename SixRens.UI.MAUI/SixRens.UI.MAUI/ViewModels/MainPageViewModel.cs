using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SixRens.UI.MAUI.Models;
using SixRens.UI.MAUI.Services.SixRens;

namespace SixRens.UI.MAUI.ViewModels
{
    public sealed partial class MainPageViewModel : ObservableObject
    {
        private readonly SixRensCore core;
        public MainPageViewModel(SixRensCore core)
        {
            this.core = core;

            CheckPluginsAndPresets();
        }
        
        private void CheckPluginsAndPresets()
        {
            DefaultPluginAndPresetRequirement = new(
                core.PluginPackageManager.插件包.Count is 0,
                core.PresetManager.预设列表.Count is 0);
        }

        [ObservableProperty]
        DefaultPluginAndPresetRequirement defaultPluginAndPresetRequirement;

        [RelayCommand]
        private async Task InstallDefaultPluginsAndPresetsAsync(
            DefaultPluginAndPresetRequirement requirement)
        {
            requirement ??= defaultPluginAndPresetRequirement;
            if (requirement.WithoutPlugins)
                _ = await core.InstallDefaultPluginsAsync();
            if (requirement.WithoutPresets)
                await core.InstallDefaultPresetsAsync();
            CheckPluginsAndPresets();
        }
    }
}
