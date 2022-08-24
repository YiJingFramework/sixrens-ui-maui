using CommunityToolkit.Maui.Views;
using Java.Lang;
using Microsoft.Maui.Storage;
using SixRens.Core.�������.���������;
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

        private ObservableCollection<�����> packageCollectionViewItemSource;

        private void SyncWithCore()
        {
            this.packageCollectionViewItemSource = new(core.PluginPackageManager.�����);
            this.packageCollectionView.ItemsSource = this.packageCollectionViewItemSource;
        }

        private async void ShowPluginPackageDetails(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var package = (�����)frame.BindingContext;
            var popupResult = (PluginPackageDetailsPopup.PopupResult)
                await this.ShowPopupAsync(new PluginPackageDetailsPopup(package));
            switch (popupResult)
            {
                case PluginPackageDetailsPopup.PopupResult.DeletionRequired:
                {
                    await Task.Factory.StartNew(() => {
                        core.PluginPackageManager.�Ƴ������(package);
                        _ = packageCollectionViewItemSource.Remove(package);
                    });
                    break;
                }
            }
        }

        private async void ClearPackages(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() => {
                var packages = core.PluginPackageManager.�����.ToArray();
                foreach (var package in packages)
                    core.PluginPackageManager.�Ƴ������(package);
                packageCollectionViewItemSource.Clear();
            });
        }

        private async void ClearPresets(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() => {
                var presets = core.PresetManager.Ԥ���б�.ToArray();
                foreach (var preset in presets)
                    core.PresetManager.ɾ��Ԥ��(preset);
            });
        }
    }
}