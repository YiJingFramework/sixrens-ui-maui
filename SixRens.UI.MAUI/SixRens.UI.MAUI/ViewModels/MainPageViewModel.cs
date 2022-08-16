using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SixRens.UI.MAUI.Services.SixRens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.ViewModels
{
    public sealed partial class MainPageViewModel : ObservableObject
    {
        private SixRensCore core;
        public MainPageViewModel(SixRensCore core)
        {
            this.core = core;
            this.PluginsCount = core.PluginPackageManager.插件包.Count;
            this.PresetsCount = core.PresetManager.预设列表.Count;
        }

        [ObservableProperty]
        int pluginsCount;

        [RelayCommand]
        async Task InstallDefaultPlugins()
        {
            await core.InstallDefaultPlugins();
            this.PluginsCount = core.PluginPackageManager.插件包.Count;
        }

        [ObservableProperty]
        int presetsCount;

        [RelayCommand]
        async Task InstallDefaultPresets()
        {
            await core.InstallDefaultPresets();
            this.PresetsCount = core.PresetManager.预设列表.Count;
        }
    }
}
