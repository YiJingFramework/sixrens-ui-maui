using CommunityToolkit.Maui.Views;
using SixRens.Core.插件管理.插件包管理;
using SixRens.UI.MAUI.Services.SixRens;
using System.Collections.ObjectModel;

namespace SixRens.UI.MAUI.Pages.PluginManagement.PackageManagement
{
    public partial class PackageManagementPage : ContentPage
    {
        private readonly IFilePicker filePicker;
        private readonly SixRensCore core;
        private readonly AppShell shell;
        public PackageManagementPage(SixRensCore core, IFilePicker filePicker, AppShell shell)
        {
            this.filePicker = filePicker;
            this.core = core;
            this.shell = shell;

            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            SyncWithCore();
        }

        private void SyncWithCore()
        {
            this.packageCollectionView.ItemsSource = null;
            this.packageCollectionView.ItemsSource = core.PluginPackageManager.插件包;
        }

        private async void OnShowDetailsClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var package = (插件包)button.BindingContext;

            var page = new PackageDetailsPage(package, core.PluginPackageManager.移除插件包, shell);
            await shell.Navigation.PushAsync(page);
        }

        private async void OnImportNewPackageClicked(object sender, EventArgs e)
        {
            messageLabel.Text = string.Empty;
            var fileResult = await filePicker.PickAsync();
            if(fileResult is not null)
            {
                try
                {
                    using var stream = await fileResult.OpenReadAsync();
                    var (p, d) = core.PluginPackageManager.从外部加载插件包(stream);
                    if(d)
                    {
                        p.Dispose();
                        messageLabel.Text = "与已有的插件包发生了冲突。";
                    }
                    else
                    {
                        messageLabel.Text = "加载成功";
                        this.SyncWithCore();
                    }
                }
                catch (Exception ex)
                {
                    messageLabel.Text =
                        $"加载失败。发生了以下异常：{Environment.NewLine}" +
                        $"{ex}";
                }
            }
        }
    }
}