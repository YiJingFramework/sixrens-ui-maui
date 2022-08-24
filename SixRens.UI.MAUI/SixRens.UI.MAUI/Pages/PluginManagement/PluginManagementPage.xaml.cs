using CommunityToolkit.Maui.Views;
using Java.Lang;
using Microsoft.Maui.Storage;
using SixRens.Core.插件管理.插件包管理;
using SixRens.UI.MAUI.Services.SixRens;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Google.Crypto.Tink.Prf;
using static Android.Graphics.Path;

namespace SixRens.UI.MAUI.Pages.PluginManagement
{
    public partial class PluginManagementPage : ContentPage
    {
        private IFilePicker filePicker;
        private SixRensCore core;
        public PluginManagementPage(SixRensCore core, IFilePicker filePicker)
        {
            this.filePicker = filePicker;
            this.core = core;

            InitializeComponent();

            SyncWithCore();
        }

        private ObservableCollection<插件包> packageCollectionViewItemSource;

        private void SyncWithCore()
        {
            this.packageCollectionViewItemSource = new(core.PluginPackageManager.插件包);
            this.packageCollectionView.ItemsSource = this.packageCollectionViewItemSource;
        }

        private async void ShowPluginPackageDetails(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var package = (插件包)frame.BindingContext;
            var popupResult = (PluginPackageDetailsPopup.PopupResult)
                await this.ShowPopupAsync(new PluginPackageDetailsPopup(package));
            switch (popupResult)
            {
                case PluginPackageDetailsPopup.PopupResult.DeletionRequired:
                {
                    await Task.Factory.StartNew(() => {
                        core.PluginPackageManager.移除插件包(package);
                        _ = packageCollectionViewItemSource.Remove(package);
                    });
                    break;
                }
            }
        }

        private async void ClearPackages(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() => {
                var packages = core.PluginPackageManager.插件包.ToArray();
                foreach (var package in packages)
                    core.PluginPackageManager.移除插件包(package);
                packageCollectionViewItemSource.Clear();
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
    }
}