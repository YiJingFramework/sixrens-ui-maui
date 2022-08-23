using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SixRens.Core.插件管理.插件包管理;
using SixRens.UI.MAUI.Services.SixRens;
using System.Collections.ObjectModel;

namespace SixRens.UI.MAUI.ViewModels
{
    public sealed partial class PluginManagementPageViewModel : ObservableObject
    {
        private readonly SixRensCore core;
        public PluginManagementPageViewModel(SixRensCore core)
        {
            this.core = core;
            this.PluginPackages = new(core.PluginPackageManager.插件包);
        }

        public ObservableCollection<插件包> PluginPackages { get; }

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
        private async Task RemovePluginPackageAsync(插件包 package)
        {
            await Task.Factory.StartNew(() => {
                core.PluginPackageManager.移除插件包(package);
            });
            PluginPackages.Remove(package);
        }

        [RelayCommand]
        private async Task RemoveAllPluginPackagesAsync()
        {
            var packages = core.PluginPackageManager.插件包.ToArray();
            await Task.Factory.StartNew(() => {
                foreach (var package in packages)
                    core.PluginPackageManager.移除插件包(package);
            });
            PluginPackages.Clear();
        }
    }
}
