using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SixRens.UI.MAUI.Services.SixRens;

namespace SixRens.UI.MAUI.ViewModels
{
    public sealed partial class PluginManagementPageViewModel : ObservableObject
    {
        private readonly SixRensCore core;
        public PluginManagementPageViewModel(SixRensCore core)
        {
            this.core = core;
        }

        [RelayCommand]
        private async Task RemoveAllPresetsAsync()
        {
            var presets = core.PresetManager.预设列表.ToArray();
            await Task.Factory.StartNew(() => {
                foreach (var preset in presets)
                    core.PresetManager.删除预设(preset);
            });
        }

        [RelayCommand]
        private async Task RemoveAllPluginPackagesAsync()
        {
            var packages = core.PluginPackageManager.插件包.ToArray();
            await Task.Factory.StartNew(() => {
                foreach (var package in packages)
                    core.PluginPackageManager.移除插件包(package);
            });
        }
    }
}
