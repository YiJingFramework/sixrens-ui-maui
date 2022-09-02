using CommunityToolkit.Maui.Views;
using Java.Lang;
using Microsoft.Maui.Storage;
using SixRens.Core.插件管理.插件包管理;
using SixRens.UI.MAUI.Services.SixRens;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Google.Crypto.Tink.Prf;
using static Android.Graphics.Path;

namespace SixRens.UI.MAUI.Pages.PluginManagement.PackageManagement
{
    public partial class PackageDetailsPage : ContentPage
    {
        private readonly 插件包 package;
        private readonly Action<插件包> deleteAction;
        private readonly AppShell shell;
        public PackageDetailsPage(插件包 package, Action<插件包> deleteAction, AppShell shell)
        {
            this.package = package;
            this.deleteAction = deleteAction;
            this.shell = shell;

            InitializeComponent();

            packageNameLabel.Text = $"{package.名称}（{package.版本号}）";

            if (package.网址 is null)
                packageWebsiteLabel.IsVisible = false;
            else
            {
                packageWebsiteLabel.IsVisible = true;
                packageWebsiteLabel.Text = package.网址;
            }
            this.threeSubjectsPlugins.ItemsSource = package.三传插件;
            this.heavenlyGeneralPlugins.ItemsSource = package.天将插件;
            this.branchAuxiliaryPlugins.ItemsSource = package.神煞插件;
            this.plateNamePlugins.ItemsSource = package.课体插件;
            this.referencePlugins.ItemsSource = package.参考插件;
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            deleteAction(package);
            _ = await shell.Navigation.PopAsync();
        }
    }
}