using CommunityToolkit.Maui.Views;
using Java.Lang;
using Microsoft.Maui.Storage;
using SixRens.Core.插件管理.插件包管理;
using SixRens.UI.MAUI.Pages.PluginManagement.PackageManagement;
using SixRens.UI.MAUI.Pages.PluginManagement.PresetManagement;
using SixRens.UI.MAUI.Services.SixRens;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Google.Crypto.Tink.Prf;
using static Android.Graphics.Path;

namespace SixRens.UI.MAUI.Pages.PluginManagement
{
    public partial class PluginManagementPage : ContentPage
    {
        private readonly IFilePicker filePicker;
        private readonly SixRensCore core;
        private readonly AppShell shell;
        public PluginManagementPage(SixRensCore core, IFilePicker filePicker, AppShell shell)
        {
            this.filePicker = filePicker;
            this.core = core;
            this.shell = shell;

            InitializeComponent();
        }

        private async void ClearPackages(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() => {
                var packages = core.PluginPackageManager.插件包.ToArray();
                foreach (var package in packages)
                    core.PluginPackageManager.移除插件包(package);
            });
        }

        private async void ClearPresets(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() => {
                var presets = core.PresetManager.预设列表.ToArray();
                foreach (var preset in presets)
                    core.PresetManager.删除预设(preset);
            });
        }

        private PackageManagementPage packageManagementPage;
        private async void OnManagePackagesClicked(object sender, EventArgs e)
        {
            if(packageManagementPage is null)
                packageManagementPage = new(core, filePicker, shell);
            await shell.Navigation.PushAsync(packageManagementPage);
        }

        private PresetManagementPage presetManagementPage;
        private async void OnManagePresetsClicked(object sender, EventArgs e)
        {
            if (presetManagementPage is null)
                presetManagementPage = new(core, filePicker, shell);
            await shell.Navigation.PushAsync(presetManagementPage);
        }
    }
}