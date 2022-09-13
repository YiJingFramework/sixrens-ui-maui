using CommunityToolkit.Maui.Views;
using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using SixRens.UI.MAUI.Services.SixRens;
using System.Collections.ObjectModel;
using static Java.Util.Jar.Attributes;

namespace SixRens.UI.MAUI.Pages.PluginManagement.PresetManagement
{
    public partial class PresetManagementPage : ContentPage
    {
        private readonly IFilePicker filePicker;
        private readonly SixRensCore core;
        private readonly AppShell shell;
        public PresetManagementPage(SixRensCore core, IFilePicker filePicker, AppShell shell)
        {
            this.filePicker = filePicker;
            this.core = core;
            this.shell = shell;

            InitializeComponent();

            presetCollectionViewItemSource = new();
            this.presetCollectionView.ItemsSource = presetCollectionViewItemSource;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            SyncWithCore();
        }

        private readonly ObservableCollection<预设> presetCollectionViewItemSource;
        private void SyncWithCore()
        {
            var newPresets = core.PresetManager.预设列表.ToDictionary(p => p.预设名);

            for (int i = 0; i < presetCollectionViewItemSource.Count; i++)
            {
                var preset = presetCollectionViewItemSource[i];
                if (newPresets.Remove(preset.预设名, out var p))
                {
                    if (!object.ReferenceEquals(preset, p))
                        presetCollectionViewItemSource[i] = p;
                }
                else
                {
                    presetCollectionViewItemSource.RemoveAt(i);
                    i--;
                }
            }

            foreach (var (_, p) in newPresets)
                presetCollectionViewItemSource.Add(p);
        }

        private async void OnShowDetailsClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var preset = (预设)button.BindingContext;

            var page = new PresetDetailsPage(
                core, preset, core.PresetManager.删除预设, shell);
            await shell.Navigation.PushAsync(page);
        }

        private async void OnImportPresetClicked(object sender, EventArgs e)
        {
            var fileResult = await filePicker.PickAsync();
            if (fileResult is null)
            {
                messageLabel.Text = "导入操作已取消";
                return;
            }
            string fileContent;
            try
            {
                using var stream = await fileResult.OpenReadAsync();
                using var reader = new StreamReader(stream);
                fileContent = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                messageLabel.Text = $"读取文件失败：{Environment.NewLine}{ex}";
                return;
            }

            var givenName = presetNameEntry.Text;
            if (string.IsNullOrWhiteSpace(givenName))
                givenName = "未命名预设";

            string name = givenName;
            for(; ; )
            {
                预设 result;

                try
                {
                    result = await core.PresetManager.导入预设文件内容(name, fileContent);
                }
                catch (Exception ex)
                {
                    messageLabel.Text = $"解析预设时发生异常：{Environment.NewLine}{ex}";
                    return;
                }

                if (result is not null)
                    break;

                name = $"{givenName}-{Guid.NewGuid():N}";
            }

            messageLabel.Text = $"成功添加预设（{name}）";
            this.SyncWithCore();
        }

        private async void OnCreateNewPresetClicked(object sender, EventArgs e)
        {
            var givenName = presetNameEntry.Text;
            if(string.IsNullOrWhiteSpace(givenName))
                givenName = "未命名预设";

            var name = givenName;
            for (; ; )
            {
                if (await core.PresetManager.新增预设(name) is not null)
                    break;
                name = $"{givenName}{Guid.NewGuid():N}";
            }

            messageLabel.Text = $"成功添加预设（{name}）";
            this.SyncWithCore();
        }
    }
}